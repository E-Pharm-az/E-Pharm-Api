using System.Text;
using EPharm.Domain.Interfaces;
using EPharm.Domain.Interfaces.Jwt;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Services;
using EPharm.Domain.Services.JwtServices;
using EPharm.Domain.Services.PharmaServices;
using EPharm.Domain.Services.ProductServices;
using EPharm.Domain.Services.UserServices;
using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.IdentityRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using EPharm.Infrastructure.Repositories.IdentityRepositories;
using EPharm.Infrastructure.Repositories.JunctionsRepositories;
using EPharm.Infrastructure.Repositories.PharmaRepositories;
using EPharm.Infrastructure.Repositories.ProductRepositories;
using EPharmApi.Health;
using EPharmApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Stripe;
using OrderService = EPharm.Domain.Services.OrderService;
using ProductService = EPharm.Domain.Services.ProductServices.ProductService;
using TokenService = EPharm.Domain.Services.JwtServices.TokenService;

namespace EPharmApi;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();

        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database health check");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

        services.AddSwaggerGen(ops =>
        {
            ops.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer 1s\""
            });

            ops.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddIdentity<AppIdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });

        services.AddCors(ops =>
            ops.AddPolicy("AllowAnyOrigins", builder => builder.AllowAnyOrigin()));
        
        StripeConfiguration.ApiKey = configuration["StripeConfig:SecretKey"];

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddDbContext<AppIdentityDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("UserDefaultConnection")));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<DbSeeder>();

        services.AddScoped<IActiveIngredientRepository, ActiveIngredientRepository>();
        services.AddScoped<IAllergyRepository, AllergyRepository>();
        services.AddScoped<IDosageFormRepository, DosageFormRepository>();
        services.AddScoped<IIndicationRepository, IndicationRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IRegulatoryInformationRepository, RegulatoryInformationRepository>();
        services.AddScoped<IRouteOfAdministrationRepository, RouteOfAdministrationRepository>();
        services.AddScoped<ISideEffectRepository, SideEffectRepository>();
        services.AddScoped<ISpecialRequirementRepository, SpecialRequirementRepository>();
        services.AddScoped<IUsageWarningRepository, UsageWarningRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IIndicationProductRepository, IndicationProductRepository>();
        services.AddScoped<IProductActiveIngredientRepository, ProductActiveIngredientRepository>();
        services.AddScoped<IProductAllergyRepository, ProductAllergyRepository>();
        services.AddScoped<IProductDosageFormRepository, ProductDosageFormRepository>();
        services.AddScoped<IProductRouteOfAdministrationRepository, ProductRouteOfAdministrationRepository>();
        services.AddScoped<IProductSideEffectRepository, ProductSideEffectRepository>();
        services.AddScoped<IProductUsageWarningRepository, ProductUsageWarningRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();

        services.AddScoped<IPharmaCompanyRepository, PharmaCompanyRepository>();
        services.AddScoped<IPharmaCompanyManagerRepository, PharmaCompanyManagerRepository>();
        services.AddScoped<IAppIdentityUserRepository, AppIdentityUserRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IManufacturerService, ManufacturerService>();
        services.AddScoped<IActiveIngredientService, ActiveIngredientService>();
        services.AddScoped<ISpecialRequirementService, SpecialRequirementService>();
        services.AddScoped<IRegulatoryInformationService, RegulatoryInformationService>();
        services.AddScoped<IAllergyService, AllergyService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IDosageFormService, DosageFormService>();
        services.AddScoped<IIndicationService, IndicationService>();
        services.AddScoped<IRouteOfAdministrationService, RouteOfAdministrationService>();
        services.AddScoped<ISideEffectService, SideEffectService>();
        services.AddScoped<IUsageWarningService, UsageWarningService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IProductImageService, ProductImageService>();

        services.AddScoped<IPharmaCompanyService, PharmaCompanyService>();
        services.AddScoped<IPharmaCompanyManagerService, PharmaCompanyManagerService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenCreationService, TokenCreationService>();
        services.AddScoped<ITokenRefreshService, TokenRefreshService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbSeeder dbSeeder)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseHealthChecks("/_health");
    
        app.UseRouting();
    
        app.UseCors("AllowAnyOrigins");
    
        app.UseAuthentication();
        app.UseAuthorization();
    
        app.UseHsts();
    
        app.UseSerilogRequestLogging();
    
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        dbSeeder.SeedSuperAdminAsync().Wait();
    }
}
