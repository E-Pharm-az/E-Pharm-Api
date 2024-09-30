using System.Text;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.JwtContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Services.Common;
using EPharm.Domain.Services.Pharma;
using EPharm.Domain.Services.Entities;
using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Interfaces.Identity;
using EPharm.Infrastructure.Interfaces.Pharma;
using EPharm.Infrastructure.Repositories.Base;
using EPharm.Infrastructure.Repositories.Identity;
using EPharm.Infrastructure.Repositories.Junctions;
using EPharm.Infrastructure.Repositories.Entities;
using EPharm.Infrastructure.Repositories.Pharma;
using EPharmApi.Health;
using EPharmApi.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using OrderService = EPharm.Domain.Services.Common.OrderService;
using ProductService = EPharm.Domain.Services.Entities.ProductService;
using TokenService = EPharm.Domain.Services.Jwt.TokenService;

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
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["accessToken"];
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddCors(ops =>
        {
            ops.AddPolicy("LocalhostPolicy", policy =>
                policy.WithOrigins("https://localhost:5270", "https://localhost:5271", "https://localhost:5272")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .WithHeaders("Content-Type", "Authorization")
            );

            ops.AddPolicy("ApiCorsPolicy", policy =>
                policy.WithOrigins("https://www.e-pharm.co", "https://e-pharm.co", "https://pms.e-pharm.co", "https://admin.e-pharm.co", "https://localhost:5270")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .WithHeaders("Content-Type", "Authorization")
            );
        });
        
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
        services.AddScoped<IWarehouseProductRepository, WarehouseProductRepository>();

        services.AddScoped<IPharmacyRepository, PharmacyRepository>();
        services.AddScoped<IPharmacyStaffRepository, PharmacyStaffRepository>();
        services.AddScoped<IAppIdentityUserRepository, AppIdentityUserRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IManufacturerService, ManufacturerService>();
        services.AddScoped<IActiveIngredientsService, ActiveIngredientsService>();
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
        services.AddScoped<ISalesService, SalesService>();

        services.AddScoped<IPharmacyService, PharmacyService>();
        services.AddScoped<IPharmacyStaffService, PharmacyStaffService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddSingleton<IEmailService, EmailService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPayPalClient, PayPalClient>();
        services.AddScoped<IOrderConfirmationEmail, OrderConfirmationEmail>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbSeeder dbSeeder,
        IEmailService emailService)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("LocalhostPolicy");
        }
        else
        {
            app.UseCors("ApiCorsPolicy");
        }

        app.UseHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSerilogRequestLogging();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        dbSeeder.SeedSuperAdminAsync().Wait();
        emailService.CompileEmail("pharmacy-invitation", "Emails/pharmacy_invitation.html").Wait();
        emailService.CompileEmail("pharmacy-staff-invitation", "Emails/pharmacy_staff_invitation.html").Wait();
        emailService.CompileEmail("change-password", "Emails/change_password.html").Wait();
        emailService.CompileEmail("confirmation", "Emails/confirmation.html").Wait();
        emailService.CompileEmail("order-confirmation", "Emails/order_confirmation.html").Wait();
    }
}
