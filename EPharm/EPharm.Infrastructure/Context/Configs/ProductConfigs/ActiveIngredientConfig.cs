using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ActiveIngredientConfig : IEntityTypeConfiguration<ActiveIngredient>
{
    public void Configure(EntityTypeBuilder<ActiveIngredient> builder)
    {
        builder.Property(ai => ai.IngredientName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ai => ai.IngredientDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(ai => ai.Products)
            .WithOne(ai => ai.ActiveIngredient)
            .HasForeignKey(ai => ai.ActiveIngredientId);

        builder.Property(ai => ai.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}