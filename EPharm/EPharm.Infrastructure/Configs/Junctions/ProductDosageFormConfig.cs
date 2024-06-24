using EPharm.Infrastructure.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Junctions;

public class ProductDosageFormConfig : IEntityTypeConfiguration<ProductDosageForm>
{
    public void Configure(EntityTypeBuilder<ProductDosageForm> builder)
    {
        builder.HasKey(pdf => new { pdf.ProductId, pdf.DosageFormId });

        builder.HasOne(pdf => pdf.Product)
            .WithMany(p => p.DosageForms)
            .HasForeignKey(pdf => pdf.ProductId);

        builder.HasOne(pdf => pdf.DosageForm)
            .WithMany(df => df.Products)
            .HasForeignKey(pdf => pdf.DosageFormId);
    }
}
