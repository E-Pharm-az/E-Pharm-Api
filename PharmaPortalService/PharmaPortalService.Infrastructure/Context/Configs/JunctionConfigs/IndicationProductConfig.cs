using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Configs.JunctionConfigs;

public class IndicationProductConfig : IEntityTypeConfiguration<IndicationProduct>
{
    public void Configure(EntityTypeBuilder<IndicationProduct> builder)
    {
        builder.HasKey(ip => new { ip.IndicationId, ip.ProductId });

        builder.HasOne(ip => ip.Indication)
            .WithMany(i => i.IndicationProducts)
            .HasForeignKey(ip => ip.IndicationId);

        builder.HasOne(ip => ip.Product)
            .WithMany(p => p.Indications)
            .HasForeignKey(ip => ip.ProductId);
    }
}
