using EPharm.Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.ProductConfigs;

public class SpecialRequirementsConfig : IEntityTypeConfiguration<SpecialRequirement>
{
    public void Configure(EntityTypeBuilder<SpecialRequirement> builder)
    {
        builder.HasOne(a => a.Pharmacy)
            .WithMany(a => a.SpecialRequirements)
            .HasForeignKey(a => a.PharmacyId);

        builder.Property(sr => sr.MinimumAgeInMonthsRequirement);

        builder.Property(sr => sr.MaximumAgeInMonthsRequirement);

        builder.Property(sr => sr.MinimumWeighInKgRequirement);

        builder.Property(sr => sr.MaximumWeighInKgRequirement);

        builder.Property(sr => sr.MedicalConditionsDescription)
            .HasMaxLength(500);

        builder.Property(sr => sr.OtherRequirementsDescription)
            .HasMaxLength(500);
        
        builder.HasMany(sr => sr.Products)
            .WithOne(sr => sr.SpecialRequirement)
            .HasForeignKey(sr => sr.SpecialRequirementsId);

        builder.Property(sr => sr.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
