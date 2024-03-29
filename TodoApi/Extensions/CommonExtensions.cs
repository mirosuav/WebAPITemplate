﻿using Microsoft.AspNetCore.Mvc;
using TodoApi.Tools;

namespace TodoApi.Extensions;

public static class CommonExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToHttpOkResult<T>(this Result<T> result)
        => result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemResult();

    public static Microsoft.AspNetCore.Http.IResult ToHttpNoContentResult<T>(this Result<T> result)
        => result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblemResult();
    public static Microsoft.AspNetCore.Http.IResult ToHttpResult<T>(this Result<T> result, Func<Result<T>, Microsoft.AspNetCore.Http.IResult> resultFactory)
        => result.IsSuccess
        ? resultFactory(result)
        : result.ToProblemResult();

    public static Microsoft.AspNetCore.Http.IResult ToProblemResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            throw new ApplicationException("No Error to detils to crete.");

        return TypedResults.Problem(result.Error.ToProblemDetails());
    }

    /// <summary>
    /// Create ProblemDetails from this error.
    /// </summary>
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7807#section-3.1"/>
    public static ProblemDetails ToProblemDetails(this Error error, string? instance = null)
    {
        var pd = new ProblemDetails
        {
            Title = error.ErrorType.ToString(),
            Detail = error.Description,
            Type = error.ErrorType.GetRFCUri(),
            Status = error.ErrorType.ToStatusCode(),
            Extensions = new Dictionary<string, object?>
            {
                {"errors", new[] { error } }
            }
        };

        if (instance is not null)
            pd.Instance = instance;

        return pd;
    }

    public static int ToStatusCode(this ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetRFCUri(this ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
}
