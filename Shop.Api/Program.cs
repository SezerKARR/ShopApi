
using Shop.Api;
using Shop.Application.JsonToSql.Converter;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureEnvironmentVariables();
builder.ConfigureDatabase();
builder.ConfigureServices();
builder.ConfigureAuthentication();
builder.ConfigureSwagger();
builder.ConfigureCors();

var app = builder.Build();

app.ConfigureMiddleware();
app.Run();
