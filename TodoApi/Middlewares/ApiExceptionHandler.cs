using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TodoApi.Extensions;
using TodoApi.Tools;

namespace TodoApi.Middlewares;
/// <summary>
/// Custom API exception handler
/// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#iexceptionhandler"/>
/// </summary>
public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> logger;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails details;

        if (exception is OperationCanceledException)
        {
            details = Error.OperationCancelled.ToProblemDetails();
            logger.LogInformation("Operation cancelled");
        }
        else
        {
            details = Error.Failure("Error.Server", exception.Message)
                .ToProblemDetails($"{httpContext.Request.Method} {httpContext.Request.Path}");

            logger.LogError(exception, "{Exception} exception occured.", typeof(Exception));
        }

        await httpContext.Response.WriteAsJsonAsync(details).FreeContext();

        return true; //exception handled
    }
}
