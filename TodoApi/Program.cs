using Microsoft.EntityFrameworkCore;
using Polly.Extensions.Http;
using Polly;
using TodoApi.Authentication;
using TodoApi.Extensions;
using TodoApi.Middlewares;
using TodoApi.Todos;
using TodoApi.WeatherApi;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer(); //Optional
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddHealthChecks()
    //.AddCheck<HealthCheckHandler>("ApiHealth")//Optional
    .AddDbContextCheck<TodoDb>();

builder.Services.AddHttpClient<WeatherApiService>()
        .AddPolicyHandler(GetHttpClientRetryPolicy());

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();
//builder.Services.AddCors();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(_ => { });
    app.UseHttpsRedirection();
    app.UseHsts();
}

//app.UseCors();
//app.UseAuthentication();
//app.UseAuthorization();

//app.UseRequestLocalization();
//app.UseCustomMiddleware();

app.MapHealthChecks("/health");
//.RequireCors();
//.RequireAuthorization();

app.MapVersionPrompt("/");

app.MapGroup("/todoitems")
    .MapTodoEndpoints()
    .AddEndpointFilter<ApiKeyEndpointFilter>();

app.MapGroup("/weather")
    .MapWeatherEndpoints();

app.Run();


//Configure resiliency policy for http calls
static IAsyncPolicy<HttpResponseMessage> GetHttpClientRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}