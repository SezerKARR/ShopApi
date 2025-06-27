namespace Shop.Api;

using System.Text.Json.Serialization;
using Application.Helpers;
using Application.JsonToSql.Converter;
using Application.Services;
using Domain.Models.Messaging;
using Domain.Models.Messaging.Smtp;
using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Messaging.RabbitMQ;
using Infrastructure.Messaging.Smtp;
using Infrastructure.Repository;
using Infrastructure.Repository.AddressEntity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

public static class ProgramExtensions {
    public static void ConfigureDatabase(this WebApplicationBuilder builder) {
        var user = Env.GetString("DB_USER");
        var password = Env.GetString("DB_PASSWORD");
        var host = Env.GetString("DB_HOST");
        Console.WriteLine($"DB_USER: {user},host:{host}, DB_PASSWORD: {(string.IsNullOrEmpty(password) ? "Empty" : "Set")}");

        var connectionString = $"Server={Env.GetString("DB_HOST")};Port={Env.GetString("DB_PORT")};Database={Env.GetString("DB_NAME")};User={user};Password={password};";


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    }


    public static void ConfigureServices(this WebApplicationBuilder builder) {
        builder.Services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddMemoryCache();
        builder.Services.AddAutoMapper(typeof(MapperProfiles));

        builder.Services.AddScoped<ITransactionScopeService, TransactionScopeService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
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
        builder.Services.AddScoped<IProductFilterValueRepository, ProductFilterValueRepository>();
        builder.Services.AddScoped<IProductFilterValueService, ProductFilterValueService>();
        builder.Services.AddScoped<IFilterValueService, FilterValueService>();
        builder.Services.AddScoped<IBrandCategoryRepository, BrandCategoryRepository>();
        builder.Services.AddScoped<IBrandCategoryService, BrandCategoryService>();
        builder.Services.AddScoped<IBrandRepository, BrandRepository>();
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<IProductSellerRepository, ProductSellerRepository>();
        builder.Services.AddScoped<IProductSellerService, ProductSellerService>();
        builder.Services.AddScoped<IStockRepository, StockRepository>();
        builder.Services.AddScoped<IStockService, StockService>();
        builder.Services.AddScoped<ISellerService, SellerService>();
        builder.Services.AddScoped<ISellerRepository, SellerRepository>();
        builder.Services.AddScoped<ICouponRepository, CouponRepository>();
        builder.Services.AddScoped<ICouponService, CouponService>();
        builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
        builder.Services.AddScoped<IProductImageService, ProductImageService>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IOrderItemService, OrderItemService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
        builder.Services.AddScoped<AddressJsonToSql>();
    }
    public static void AddInfrastructure(this WebApplicationBuilder builder) {
        builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
        builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddHostedService<EmailQueueConsumer>();

    }
    public static void ConfigureAuthentication(this WebApplicationBuilder builder) {
        var GOOGLE_CLIENT_ID = Env.GetString("GOOGLE_CLIENT_ID");
        Console.WriteLine($"GOOGLE_CLIENT_ID {GOOGLE_CLIENT_ID}");
        builder.Services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
                options.ClientId = Env.GetString("GOOGLE_CLIENT_ID");
                options.ClientSecret = Env.GetString("GOOGLE_CLIENT_SECRET");
                // options.CallbackPath = "/signin-google";
            });
    }

    public static void ConfigureSwagger(this WebApplicationBuilder builder) {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            options.AddSecurityDefinition("Google", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please log in using Google",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
    public static void ConfigureCors(this WebApplicationBuilder builder) {
        builder.Services.AddCors(options => {
            options.AddPolicy("configurePolicy", configurePolicy =>
                configurePolicy.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

    }
    public static void ConfigureMiddleware(this WebApplication app) {
        app.UseCors("configurePolicy");
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
            RequestPath = "/uploads"
        });

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}