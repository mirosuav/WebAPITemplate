using TodoApi.Extensions;
using TodoApi.Todos;
using TodoApi.Tools;

namespace TodoApi.WeatherApi;

public static class WeatherEndpoints
{
    public static RouteGroupBuilder MapWeatherEndpoints(this RouteGroupBuilder group)
    {
        //Return weather by location
        group.MapGet("/{location}", 
            static async (string location, WeatherApiService weatherService, CancellationToken ct)
            => (await weatherService.GetCurrentWeather(location, ct).FreeContext())
            .ToHttpOkResult());

        return group;
    }
}
