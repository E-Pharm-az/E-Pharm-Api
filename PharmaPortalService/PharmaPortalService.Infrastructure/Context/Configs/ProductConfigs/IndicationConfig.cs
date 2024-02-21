using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class IndicationConfig : IEntityTypeConfiguration<Indication>
{
    public void Configure(EntityTypeBuilder<Indication> builder)
    {
        builder.Property(i => i.IndicationsName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.IndicationsDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
