using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Configs.JunctionConfigs;

public class ProductUsageWarningConfig : IEntityTypeConfiguration<ProductUsageWarning>
{
    public void Configure(EntityTypeBuilder<ProductUsageWarning> builder)
    {
        builder.HasKey(puw => new { puw.ProductId, puw.UsageWarningId });

        builder.HasOne(puw => puw.Product)
            .WithMany(p => p.UsageWarnings)
            .HasForeignKey(puw => puw.ProductId);

        builder.HasOne(puw => puw.UsageWarning)
            .WithMany(uw => uw.Products)
            .HasForeignKey(puw => puw.UsageWarningId);
    }
}