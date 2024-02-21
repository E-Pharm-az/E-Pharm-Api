using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class SpecialRequirementsConfig : IEntityTypeConfiguration<SpecialRequirements>
{
    public void Configure(EntityTypeBuilder<SpecialRequirements> builder)
    {
        builder.Property(sr => sr.MinimumAgeInMonthsRequirement)
            .IsRequired();

        builder.Property(sr => sr.MaximumAgeInMonthsRequirement)
            .IsRequired();

        builder.Property(sr => sr.MinimumWeighInKgRequirement)
            .IsRequired();

        builder.Property(sr => sr.MaximumWeighInKgRequirement)
            .IsRequired();

        builder.Property(sr => sr.MedicalConditionsDescription)
            .HasMaxLength(500);

        builder.Property(sr => sr.OtherRequirementsDescription)
            .HasMaxLength(500);

        builder.Property(sr => sr.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
