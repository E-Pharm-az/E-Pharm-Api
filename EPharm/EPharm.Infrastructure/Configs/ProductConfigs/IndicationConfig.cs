using EPharm.Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.ProductConfigs;

public class IndicationConfig : IEntityTypeConfiguration<Indication>
{
    public void Configure(EntityTypeBuilder<Indication> builder)
    {
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.Description)
            .HasMaxLength(500);
        
        builder.HasMany(i => i.Products)
            .WithOne(si => si.Indication)
            .HasForeignKey(si => si.IndicationId);

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
