using EPharm.Domain.Interfaces;
using EPharm.Domain.Services;
using EPharm.Infrastructure.Interfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories;
using EPharm.Infrastructure.Repositories.JunctionsRepositories;
using EPharm.Infrastructure.Repositories.ProductRepositories;

namespace EPharmApi;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IPharmaCompanyRepository, PharmaCompanyRepository>();
        services.AddScoped<IPharmaCompanyManagerRepository, PharmaCompanyManagerRepository>();

        services.AddScoped<IActiveIngredientRepository, ActiveIngredientRepository>();
        services.AddScoped<IAllergyRepository, AllergyRepository>();
        services.AddScoped<IDosageFormRepository, DosageFormRepository>();
        services.AddScoped<IIndicationRepository, IIndicationRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IRegulatoryInformationRepository, RegulatoryInformationRepository>();
        services.AddScoped<IRouteOfAdministrationRepository, RouteOfAdministrationRepository>();
        services.AddScoped<ISideEffectRepository, SideEffectRepository>();
        services.AddScoped<ISpecialRequirementRepository, SpecialRequirementRepository>();
        services.AddScoped<IUsageWarningRepository, UsageWarningRepository>();

        services.AddScoped<IIndicationProductRepository, IndicationProductRepository>();
        services.AddScoped<IProductActiveIngredientRepository, ProductActiveIngredientRepository>();
        services.AddScoped<IProductAllergyRepository, ProductAllergyRepository>();
        services.AddScoped<IProductDosageFormRepository, ProductDosageFormRepository>();
        services.AddScoped<IProductRouteOfAdministrationRepository, ProductRouteOfAdministrationRepository>();
        services.AddScoped<IProductSideEffectRepository, ProductSideEffectRepository>();
        services.AddScoped<IProductUsageWarningRepository, ProductUsageWarningRepository>();

        services.AddScoped<IPharmaCompanyService, PharmaCompanyService>();
        services.AddScoped<IPharmaCompanyManagerService, PharmaCompanyManagerService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
