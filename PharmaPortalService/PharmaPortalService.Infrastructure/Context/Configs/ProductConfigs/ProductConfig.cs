using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.ProductDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.StrengthMg)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ContraindicationsDescription)
            .HasMaxLength(500);

        builder.Property(p => p.StorageConditionDescription)
            .HasMaxLength(500);

        builder.Property(p => p.ManufacturingDate)
            .HasColumnType("date");

        builder.Property(p => p.ExpiryDate)
            .HasColumnType("date");

        builder.Property(p => p.Stock)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired();

        builder.Property(p => p.BatchNumber)
            .HasMaxLength(50);

        builder.Property(p => p.Barcode)
            .HasMaxLength(50);

        builder.Property(p => p.PackagingWidth)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PackagingHeight)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PackagingLength)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PackagingWeight)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
