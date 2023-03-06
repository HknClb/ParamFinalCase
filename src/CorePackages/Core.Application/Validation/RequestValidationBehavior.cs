﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Core.Application.Validation;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    // All validators that about CQRS request
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);
        List<ValidationFailure> failures = _validators
                                           .Select(validator => validator.Validate(context))
                                           .SelectMany(result => result.Errors)
                                           .Where(failure => failure != null)
                                           .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return next();
    }
}