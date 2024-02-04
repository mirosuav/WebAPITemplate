using Microsoft.AspNetCore.Mvc;
using TodoApi.Infrastructure;

namespace TodoApi.Extensions;

public static class CommonExtensions
{
    public static IResult ToProblemResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            throw new ApplicationException("No Error to detils to crete.");
        
        return TypedResults.Problem(result.Error.ToProblemDetails());
    }

    /// <summary>
    /// Create ProblemDetails from this error.
    /// </summary>
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7807#section-3.1"/>
    public static ProblemDetails ToProblemDetails(this ApiError error)
        => new()
        {
            Title = error.ErrorType.ToString(),
            Detail = error.Description,
            Type = error.ErrorType.ToRFCType(),
            Status = error.ErrorType.ToStatusCode(),
            Extensions = new Dictionary<string, object?>
            {
                {"errors", new[] { error } }
            }
        };

    public static int ToStatusCode(this ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };


    public static string ToRFCType(this ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
}
