using EPharm.Infrastructure.Context.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.JunctionConfigs;

public class ProductAllergyConfig : IEntityTypeConfiguration<ProductAllergy>
{
    public void Configure(EntityTypeBuilder<ProductAllergy> builder)
    {
        builder.HasKey(pa => new { pa.ProductId, pa.AllergyId });

        builder.HasOne(pa => pa.Product)
            .WithMany(p => p.Allergies)
            .HasForeignKey(pa => pa.ProductId);

        builder.HasOne(pa => pa.Allergy)
            .WithMany(a => a.Products)
            .HasForeignKey(pa => pa.AllergyId);
    }
}
