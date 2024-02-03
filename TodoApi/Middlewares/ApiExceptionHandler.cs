using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Extensions;
using TodoApi.Infrastructure;

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
        logger.LogError(exception, "Unexpected exception occured.");

        await httpContext.Response.WriteAsJsonAsync(
            ApiError.ServerError.ToProblemDetails(),
            typeof(ProblemDetails),
            cancellationToken);

        return true; //exception handled
    }
}
