using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TodoApi.Authentication;
using TodoApi.Endpoints;
using TodoApi.Infrastructure;
using TodoApi.Middlewares;

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware?view=aspnetcore-8.0
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer(); //Optional
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddHealthChecks()
    //.AddCheck<HealthCheckHandler>("ApiHealth")//Optional
    .AddDbContextCheck<TodoDb>();

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>(); 
//builder.Services.AddProblemDetails(); 

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

app.UseRequestLocalization();
//app.UseCustomMiddleware();

app.MapHealthChecks("/health");
//.RequireCors();
//.RequireAuthorization();

app.MapVersionPrompt("/");

app.MapGroup("/todoitems")
    .MapTodoEndpoints()
    .AddEndpointFilter<ApiKeyEndpointFilter>();

app.Run();

