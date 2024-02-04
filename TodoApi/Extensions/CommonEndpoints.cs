using Microsoft.AspNetCore.Routing.Patterns;
using System.Diagnostics;
using System.Reflection;

namespace TodoApi.Extensions;

public static class CommonEndpoints
{
    public static RouteHandlerBuilder MapVersionPrompt(this IEndpointRouteBuilder builder, string pattern)
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location);
        return builder.MapGet(pattern, () => $"ASP.NET CORE Web API template ver. {versionInfo.ProductVersion}");
    }

}
