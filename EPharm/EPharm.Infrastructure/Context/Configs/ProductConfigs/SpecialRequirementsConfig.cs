using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class SpecialRequirementsConfig : IEntityTypeConfiguration<SpecialRequirement>
{
    public void Configure(EntityTypeBuilder<SpecialRequirement> builder)
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
        
        builder.HasMany(sr => sr.Products)
            .WithOne(sr => sr.SpecialRequirement)
            .HasForeignKey(sr => sr.SpecialRequirementsId);

        builder.Property(sr => sr.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
