using System.Text.Json.Serialization;
using TodoApi.Tools;

namespace TodoApi.WeatherApi;

public record WeatherApiError(WeatherApiErrorDetails Error)
    : Error(Error.Code.ToString(), Error.Message, ErrorType.Validation);

public record WeatherApiErrorDetails(int Code, string Message);

public record LocationWeather
{
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("current")]
    public WeatherData? Weather { get; set; }
}

public record Location
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("tz_id")]
    public string? TimeZone { get; set; }

    [JsonPropertyName("lat")]
    public double Lattitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("localtime")]
    public string? LocalTime { get; set; }
}

public record WeatherData
{
    [JsonPropertyName("last_updated")]
    public string? LastUpdated { get; set; }

    [JsonPropertyName("temp_c")]
    public double TempC { get; set; }
}