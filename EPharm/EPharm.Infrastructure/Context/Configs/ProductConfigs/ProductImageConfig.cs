using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(pi => pi.ImageUrl)
            .IsRequired();

        builder.Property(pi => pi.UpdatedAt)
            .HasColumnType("timestamp");

        builder.Property(pi => pi.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
