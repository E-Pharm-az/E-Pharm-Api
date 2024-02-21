using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class DosageFormConfig : IEntityTypeConfiguration<DosageForm>
{
    public void Configure(EntityTypeBuilder<DosageForm> builder)
    {
        builder.HasKey(df => df.Id);

        builder.Property(df => df.DosageFormName)
            .IsRequired()
            .HasMaxLength(255);
    }
}
