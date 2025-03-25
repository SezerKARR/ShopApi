using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShopApi.Data;
using ShopApi.Helpers;
using ShopApi.Interface;
using ShopApi.Repository;
using ShopApi.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

Env.Load(".env.development");

var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                       $"Port={Env.GetString("DB_PORT")};" +
                       $"Database={Env.GetString("DB_NAME")};" +
                       $"Username={Env.GetString("DB_USER")};" +
                       $"Password={Env.GetString("DB_PASSWORD")};";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    }).AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
        options.ClientId = Env.GetString("GOOGLE_CLIENT_ID");
        options.ClientSecret = Env.GetString("GOOGLE_CLIENT_SECRET");
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("configurePolicy", configurePolicy =>
            configurePolicy.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    // Remove JWT security definition and requirement for Google authentication
    option.AddSecurityDefinition("Google", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please log in using Google",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Google"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(MapperProfiles));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBasketItemService, BasketItemService>();
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMainCategoryService, MainCategoryService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMainCategoryRepository, MainCategoryRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<IFilterValueRepository, FilterValueRepository>();
var app = builder.Build();
app.UseCors("configurePolicy");
app.Use(async (context, next) =>
{
    context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin-allow-popups";  // Popup'lara izin ver
    context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";  // COEP başlığını da ayarlıyoruz
    await next();
});
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();  
app.UseHttpsRedirection();  

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
