using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Configs.JunctionConfigs;

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
