using Microsoft.EntityFrameworkCore;
using TodoApi.Authentication;
using TodoApi.Extensions;
using TodoApi.Middlewares;
using TodoApi.Todos;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoDb"));

builder.Services.AddHealthChecks().AddDbContextCheck<TodoDb>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAPI_KEY();

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(_ => { });
    app.UseHttpsRedirection();
    app.UseHsts();
}


//app.UseRequestLocalization();
//app.UseCustomMiddleware();

app.MapHealthChecks("/health");

app.MapVersionPrompt("/");

app.MapGroup("/todoitems")
    .MapTodoEndpoints()
    .AddEndpointFilter<ApiKeyEndpointFilter>();

app.Run();

