using EPharm.Infrastructure.Context.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Junctions;

public class IndicationProductConfig : IEntityTypeConfiguration<IndicationProduct>
{
    public void Configure(EntityTypeBuilder<IndicationProduct> builder)
    {
        builder.HasKey(ip => new { ip.IndicationId, ip.ProductId });

        builder.HasOne(ip => ip.Indication)
            .WithMany(i => i.Products)
            .HasForeignKey(ip => ip.IndicationId);

        builder.HasOne(ip => ip.Product)
            .WithMany(p => p.Indications)
            .HasForeignKey(ip => ip.ProductId);
    }
}
