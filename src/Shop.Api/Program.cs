
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Shop.Api;
using Shop.Application.JsonToSql.Converter;
using Shop.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    Env.Load("../../.env.docker");
}
else
{
    Env.Load("../../.env");
}
builder.ConfigureDatabase();
builder.ConfigureServices();
builder.ConfigureAuthentication();
builder.ConfigureSwagger();
builder.ConfigureCors();
builder.AddInfrastructure();
var app = builder.Build();

app.ConfigureMiddleware();
app.Run();
