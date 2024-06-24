using EPharm.Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.ProductConfigs;

public class RegulatoryInformationConfig : IEntityTypeConfiguration<RegulatoryInformation>
{
    public void Configure(EntityTypeBuilder<RegulatoryInformation> builder)
    {
        builder.Property(ri => ri.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(ri => ri.Pharmacy)
            .WithMany(ri => ri.RegulatoryInformations)
            .HasForeignKey(ri => ri.PharmaCompanyId);

        builder.Property(ri => ri.ApprovalDate)
            .HasColumnType("date");
        
        builder.HasMany(ri => ri.Product)
            .WithOne(ri => ri.RegulatoryInformation)
            .HasForeignKey(ri => ri.RegulatoryInformationId)
            .IsRequired();

        builder.Property(ri => ri.Certification)
            .HasMaxLength(255);

        builder.Property(ri => ri.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
