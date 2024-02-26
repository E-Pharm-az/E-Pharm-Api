using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ManufacturerConfig : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.Property(m => m.ManufacturerName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.ManufacturerLocation)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
