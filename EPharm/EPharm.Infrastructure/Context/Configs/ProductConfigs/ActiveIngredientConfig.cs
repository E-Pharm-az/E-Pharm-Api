using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ActiveIngredientConfig : IEntityTypeConfiguration<ActiveIngredient>
{
    public void Configure(EntityTypeBuilder<ActiveIngredient> builder)
    {
        builder.Property(ai => ai.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(ai => ai.PharmaCompany)
            .WithMany(ai => ai.ActiveIngredients)
            .HasForeignKey(ai => ai.PharmaCompanyId);
        
        builder.Property(ai => ai.Description)
            .HasMaxLength(500);

        builder.HasMany(ai => ai.Products)
            .WithOne(ai => ai.ActiveIngredient)
            .HasForeignKey(ai => ai.ActiveIngredientId);

        builder.Property(ai => ai.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
