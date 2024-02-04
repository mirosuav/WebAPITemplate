
namespace TodoApi.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
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
            .TryGetValue(AuthenticationConst.ApiKeyHeaderName, out var requestApiKey))
        {
            return TypedResults.Unauthorized();
        }
        
        var _apiKey = _configuration.GetValue<string>("API_KEY");
        if (_apiKey is null or [])
        {
            logger.LogError("Invalid configuration: no API_KEY environment variable defined!");
            throw new ApplicationException("Invalid application configuration");            
        }

        if (!_apiKey.Equals(requestApiKey))
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}
