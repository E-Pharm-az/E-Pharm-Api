using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class DosageFormConfig : IEntityTypeConfiguration<DosageForm>
{
    public void Configure(EntityTypeBuilder<DosageForm> builder)
    {
        builder.HasKey(df => df.Id);
        
        builder.HasMany(df => df.Products)
            .WithOne(df => df.DosageForm)
            .HasForeignKey(df => df.DosageFormId);

        builder.Property(df => df.DosageFormName)
            .IsRequired()
            .HasMaxLength(255);
    }
}
