using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.CrossCuttingConcerns.Exceptions.Validation;

public class ValidationProblemDetails : ProblemDetails
{
    public object? Errors { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);
}