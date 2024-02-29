using System.Diagnostics;
using System.Reflection;

namespace TodoApi.Extensions;

public static class CommonEndpoints
{
    public static RouteHandlerBuilder MapVersionPrompt(this IEndpointRouteBuilder builder, string pattern)//TODO It is right place for this functionality?
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location);
        return builder.MapGet(pattern, () => $"Web API template ver. {versionInfo.ProductVersion}");
    }
}
