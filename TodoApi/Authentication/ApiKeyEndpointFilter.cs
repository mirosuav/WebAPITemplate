
namespace TodoApi.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    public const string ApiKeyHeaderName = "X-API-KEY";
    public const string ApiKeyEnvName = "API_KEY";

    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyEndpointFilter> logger;

    public ApiKeyEndpointFilter(IConfiguration configuration, ILogger<ApiKeyEndpointFilter> logger)
    {
        _configuration = configuration;
        this.logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers
            .TryGetValue(ApiKeyHeaderName, out var requestApiKey))
        {
            return TypedResults.Unauthorized();
        }
        
        var _apiKey = _configuration.GetValue<string>(ApiKeyEnvName);
        if (_apiKey is null or [])
        {
            logger.LogError("Invalid configuration: no {API_KEY} environment variable defined!", ApiKeyEnvName);
            throw new ApplicationException("Invalid application configuration");            
        }

        if (!_apiKey.Equals(requestApiKey))
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}
