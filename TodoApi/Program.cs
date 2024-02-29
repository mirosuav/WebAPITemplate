using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using TodoApi.Authentication;
using TodoApi.Extensions;
using TodoApi.Middlewares;
using TodoApi.Todos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
//builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoDb"));

builder.Services.AddHealthChecks().AddDbContextCheck<TodoDb>();

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGenWithAPI_KEY();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var initScope = app.Services.CreateScope())
    {
        var db = initScope.ServiceProvider.GetRequiredService<TodoDb>();
        db.Database.EnsureCreated();
        db.Seed();
    }
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
    app.UseExceptionHandler(opt => { });
}


//app.UseRequestLocalization();
//app.UseCustomMiddleware();

app.MapHealthChecks("/health");

app.MapVersionPrompt("/");

var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(1.0)
                    .ReportApiVersions()
                    .Build();

app.MapGroup("/todoitems")
    .MapTodoEndpoints()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0)
    .AddEndpointFilter<ApiKeyEndpointFilter>();


app.Run();

