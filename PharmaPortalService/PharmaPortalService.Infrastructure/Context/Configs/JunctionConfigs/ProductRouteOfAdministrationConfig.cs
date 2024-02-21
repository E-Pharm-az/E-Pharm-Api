using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Configs.JunctionConfigs;

public class ProductRouteOfAdministrationConfig : IEntityTypeConfiguration<ProductRouteOfAdministration>
{
    public void Configure(EntityTypeBuilder<ProductRouteOfAdministration> builder)
    {
        builder.HasKey(pra => new { pra.ProductId, pra.RouteOfAdministrationId });

        builder.HasOne(pra => pra.Product)
            .WithMany(p => p.RouteOfAdministrations)
            .HasForeignKey(pra => pra.ProductId);

        builder.HasOne(pra => pra.RouteOfAdministration)
            .WithMany(ra => ra.Products)
            .HasForeignKey(pra => pra.RouteOfAdministrationId);
    }
}