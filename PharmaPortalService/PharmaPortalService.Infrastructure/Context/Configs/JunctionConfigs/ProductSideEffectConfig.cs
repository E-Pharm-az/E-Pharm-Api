using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Configs.JunctionConfigs;

public class ProductSideEffectConfig : IEntityTypeConfiguration<ProductSideEffect>
{
    public void Configure(EntityTypeBuilder<ProductSideEffect> builder)
    {
        builder.HasKey(pse => new { pse.ProductId, pse.SideEffectId });

        builder.HasOne(pse => pse.Product)
            .WithMany(p => p.SideEffects)
            .HasForeignKey(pse => pse.ProductId);

        builder.HasOne(pse => pse.SideEffect)
            .WithMany(se => se.Products)
            .HasForeignKey(pse => pse.SideEffectId);
    }
}
