using Microsoft.EntityFrameworkCore;
using TodoApi.Endpoints;
using TodoApi.Infrastructure;

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware?view=aspnetcore-8.0
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); //Optional
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheckHandler>("ApiHealth")//Optional
    .AddDbContextCheck<TodoDb>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.MapHealthChecks("/health")
    .RequireCors();
                   //.RequireAuthorization();

app.MapVersionPrompt("/");

app.MapGroup("/todoitems").MapTodoEndpoints();

app.Run();
