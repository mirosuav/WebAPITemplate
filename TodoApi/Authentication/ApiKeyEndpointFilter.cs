
namespace TodoApi.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers
            .TryGetValue(AuthenticationConst.ApiKeyHeaderName, out var requestApiKey))
        {
            return TypedResults.Unauthorized();
        }
        var _apiKey = _configuration.GetValue<string>("API_KEY") 
            ?? throw new ApplicationException("Invalid configuration: no API KEY defined!");

        if (!_apiKey.Equals(requestApiKey))
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}
