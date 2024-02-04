using Microsoft.Extensions.Logging;
using System.Text.Json;
using TodoApi.Tools;

namespace TodoApi.WeatherApi;

public sealed class WeatherApiService
{
    const string API_KEY_NAME = "WEATHER_API_KEY";
    const string CurrentWeatherUrl = "https://api.weatherapi.com/v1/current.json";

    private readonly IConfiguration configuration;
    private readonly HttpClient _http;
    private readonly ILogger<WeatherApiService> _logger;
    private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public WeatherApiService(
        IConfiguration configuration,
        HttpClient http,
        ILogger<WeatherApiService> logger)
    {
        this.configuration = configuration;
        _http = http;
        _logger = logger;

    }

    public async Task<Result<LocationWeather>> GetCurrentWeather(string location, CancellationToken token = default)
    {
        try
        {
            var key = configuration.GetValue<string>(API_KEY_NAME);
            if (key is null or [])
            {
                _logger.LogError("Invalid configuration: no WEATHER_API_KEY environment variable defined!");
                return Error.ServerError;
            }

            if (location is null or [])
                return Error.NullValue;

            var qs = QueryString.Create("key", key);
            qs = qs.Add("aqi", "no"); //no air quality data
            qs = qs.Add("q", location);

            var result = await _http.GetAsync(CurrentWeatherUrl + qs.ToUriComponent()).FreeContext();

            if (!result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<WeatherApiError>(jsonOptions, token).FreeContext()
                             ?? throw new ApplicationException("Invalid WeatherAPI response");
            }

            return await result.Content.ReadFromJsonAsync<LocationWeather>(token)
                   ?? throw new ApplicationException("Invalid WeatherAPI response");
        }
        catch (OperationCanceledException)
        {
            return Error.OperationCancelled;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occured");
            return Error.Failure("WeatherApi.Error", ex.Message);
        }
    }

}
