// using System.Text.Json.Serialization;
// using Microsoft.OpenApi.Models;
// using DotNetEnv;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.Google;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.FileProviders;
// using Shop.Application.Helpers;
// using Shop.Application.Services;
// using Shop.Infrastructure.Data;
// using Shop.Infrastructure.Repository;
//
// var builder = WebApplication.CreateBuilder(args);
//
// Env.Load(".env.development");
//
// var connectionString = $"Host={Env.GetString("DB_HOST")};" +
//                        $"Port={Env.GetString("DB_PORT")};" +
//                        $"Database={Env.GetString("DB_NAME")};" +
//                        $"Username={Env.GetString("DB_USER")};" +
//                        $"Password={Env.GetString("DB_PASSWORD")};";
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//
// builder.Services.AddControllers()
//     .AddJsonOptions(x =>
//         x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// builder.Services.AddAuthentication(options => {
//         options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//     }).AddCookie()
//     .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
//         options.ClientId = Env.GetString("GOOGLE_CLIENT_ID");
//         options.ClientSecret = Env.GetString("GOOGLE_CLIENT_SECRET");
//     });
//
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("configurePolicy", configurePolicy =>
//             configurePolicy.WithOrigins("http://localhost:5173")
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials());
// });
//
//
//
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(option =>
// {
//     option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
//     // Remove JWT security definition and requirement for Google authentication
//     option.AddSecurityDefinition("Google", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please log in using Google",
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "Bearer"
//     });
//     option.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Google"
//                 }
//             },
//             new string[] { }
//         }
//     });
// });
// builder.Services.AddMemoryCache();
// builder.Services.AddAutoMapper(typeof(MapperProfiles));
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IBasketItemService, BasketItemService>();
// builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
// builder.Services.AddScoped<IBasketRepository, BasketRepository>();
// builder.Services.AddScoped<IBasketService, BasketService>();
// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<ICategoryService, CategoryService>();
// builder.Services.AddScoped<ICommentService, CommentService>();
// builder.Services.AddScoped<IMainCategoryService, MainCategoryService>();
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ICommentRepository, CommentRepository>();
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// builder.Services.AddScoped<IMainCategoryRepository, MainCategoryRepository>();
// builder.Services.AddScoped<IFilterRepository, FilterRepository>();
// builder.Services.AddScoped<IFilterService, FilterService>();
// builder.Services.AddScoped<IFilterValueRepository, FilterValueRepository>();
// builder.Services.AddScoped<IProductFilterValueRepository, ProductFilterValueRepository>();
// builder.Services.AddScoped<IProductFilterValueService, ProductFilterValueService>();
// builder.Services.AddScoped<IFilterValueService, FilterValueService>();
// builder.Services.AddScoped<IBrandCategoryRepository, BrandCategoryRepository>();
// builder.Services.AddScoped<IBrandCategoryService, BrandCategoryService>();
// builder.Services.AddScoped<IBrandRepository, BrandRepository>();
// builder.Services.AddScoped<IBrandService, BrandService>();
// builder.Services.AddScoped<IProductSellerRepository, ProductSellerRepository>();
// builder.Services.AddScoped<IProductSellerService, ProductSellerService>();
// builder.Services.AddScoped<IStockRepository, StockRepository>();
// builder.Services.AddScoped<IStockService, StockService>();
// builder.Services.AddScoped<ISellerService, SellerService>();
// builder.Services.AddScoped<ISellerRepository, SellerRepository>();
// builder.Services.AddScoped<ICouponRepository, CouponRepository>();
// builder.Services.AddScoped<ICouponService, CouponService>();
// builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
// builder.Services.AddScoped<IProductImageService, ProductImageService>();
// var app = builder.Build();
// app.UseCors("configurePolicy");
// app.Use(async (context, next) =>
// {
//     context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin-allow-popups";  // Popup'lara izin ver
//     context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";  // COEP başlığını da ayarlıyoruz
//     await next();
// });
// app.UseRouting();
// app.UseSwagger();
// app.UseSwaggerUI();
//
// app.UseHttpsRedirection();  
// app.UseHttpsRedirection();  
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//     Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
//     RequestPath = "/uploads"
// });
//
// app.UseAuthentication();
//
// app.UseAuthorization();
// app.MapControllers();
//
// app.Run();
using Shop.Api;

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
