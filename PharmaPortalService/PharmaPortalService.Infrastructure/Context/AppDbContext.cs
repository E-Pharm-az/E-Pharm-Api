using Microsoft.EntityFrameworkCore;
using PharmaPortalService.Infrastructure.Context.Entities;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PharmaCompany> PharmaCompanies { get; set; }
    public DbSet<PharmaCompanyManager> PharmaCompanyManagers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    public DbSet<ActiveIngredient> ActiveIngredients { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<DosageForm> DosageForms { get; set; }
    public DbSet<Indication> Indications { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<RegulatoryInformation> RegulatoryInformations { get; set; }
    public DbSet<RouteOfAdministration> RouteOfAdministrations { get; set; }
    public DbSet<SideEffect> SideEffects { get; set; }
    public DbSet<SpecialRequirement> SpecialRequirements { get; set; }
    public DbSet<UsageWarning> UsageWarnings { get; set; }
    
    public DbSet<IndicationProduct> IndicationProducts { get; set; }
    public DbSet<ProductActiveIngredient> ProductActiveIngredients { get; set; }
    public DbSet<ProductAllergy> ProductAllergies { get; set; }
    public DbSet<ProductDosageForm> ProductDosageForms { get; set; }
    public DbSet<ProductRouteOfAdministration> ProductRouteOfAdministrations { get; set; }
    public DbSet<ProductSideEffect> ProductSideEffects { get; set; }
    public DbSet<ProductUsageWarning> ProductUsageWarnings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
