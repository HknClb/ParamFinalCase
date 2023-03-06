using Core.CrossCuttingConcerns.Exceptions.Business;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Core.CrossCuttingConcerns.Exceptions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
                _logger.LogError($"[ExceptionMiddleware]: Request: {context.Request.Path} [{context.Request.Method}]. Status Code: {context.Response.StatusCode} Error Type: {exception.GetType()}");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(BusinessException))
                return CreateBusinessException(context, exception);

            if (exception.GetType() == typeof(ValidationException))
                return CreateValidationException(context, exception);

            return CreateInternalException(context, exception);
        }

        private Task CreateBusinessException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(new BusinessProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business Exception",
                Detail = exception.Message
            }.ToString());
        }

        private Task CreateValidationException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            object errors = ((ValidationException)exception).Errors;

            return context.Response.WriteAsync(new Validation.ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error(s)",
                Errors = errors
            }.ToString());
        }

        private Task CreateInternalException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Exception",
                Detail = "An unexpected error occurred"
            }.ToString());
        }
    }
}
