using Core.Application.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoApp.Application.Features.ShoppingLists.Rules;

namespace TodoApp.Application
{
    public static class ApplicationServiceRegistrations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);
            services.AddMediatR(config =>
            {
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
                config.RegisterServicesFromAssembly(assembly);
            });
            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<ShoppingListBusinessRules>();

            return services;
        }
    }
}
