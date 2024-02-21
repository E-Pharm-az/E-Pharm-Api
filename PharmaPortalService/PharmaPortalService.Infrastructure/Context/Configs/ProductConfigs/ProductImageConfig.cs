using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(pi => pi.ImageUrl)
            .IsRequired();

        builder.Property(pi => pi.UpdatedAt)
            .HasColumnType("datetime");

        builder.Property(pi => pi.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
