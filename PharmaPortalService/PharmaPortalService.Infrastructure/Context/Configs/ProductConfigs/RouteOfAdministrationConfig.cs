using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class RouteOfAdministrationConfig : IEntityTypeConfiguration<RouteOfAdministration>
{
    public void Configure(EntityTypeBuilder<RouteOfAdministration> builder)
    {
        builder.Property(ra => ra.Description)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasMany(re => re.Products)
            .WithOne(re => re.RouteOfAdministration)
            .HasForeignKey(re => re.RouteOfAdministrationId);

        builder.Property(ra => ra.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
