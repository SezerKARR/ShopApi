
using DotNetEnv;
using Shop.Api;
using Shop.Application.JsonToSql.Converter;

var builder = WebApplication.CreateBuilder(args);
Env.Load(".env.development"); 
builder.ConfigureEnvironmentVariables();
builder.ConfigureDatabase();
builder.ConfigureServices();
builder.ConfigureAuthentication();
builder.ConfigureSwagger();
builder.ConfigureCors();
builder.AddInfrastructure();
var app = builder.Build();

app.ConfigureMiddleware();
app.Run();
