using EPharm.Infrastructure.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Junctions;

public class ProductActiveIngredientConfig : IEntityTypeConfiguration<ProductActiveIngredient>
{
    public void Configure(EntityTypeBuilder<ProductActiveIngredient> builder)
    {
        builder.HasKey(pai => new { pai.ProductId, pai.ActiveIngredientId });

        builder.HasOne(pai => pai.Product)
            .WithMany(p => p.ActiveIngredients)
            .HasForeignKey(pai => pai.ProductId);

        builder.HasOne(pai => pai.ActiveIngredient)
            .WithMany(ai => ai.Products)
            .HasForeignKey(pai => pai.ActiveIngredientId);

        builder.Property(pai => pai.Quantity)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}
