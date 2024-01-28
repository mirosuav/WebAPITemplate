using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TodoApi.Endpoints;
using TodoApi.Infrastructure;
using TodoApi.Model;

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware?view=aspnetcore-8.0
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); //Optional
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "Todo API - ASP.NET CORE Web API template");

app.MapGroup("/todoitems").MapTodoEndpoints();

//app.Run(context =>
//{
//    context.Response.StatusCode = 404;
//    return Task.CompletedTask;
//});

app.Run();
