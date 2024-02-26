using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class RegulatoryInformationConfig : IEntityTypeConfiguration<RegulatoryInformation>
{
    public void Configure(EntityTypeBuilder<RegulatoryInformation> builder)
    {
        builder.Property(ri => ri.RegulatoryStandards)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ri => ri.ApprovalDate)
            .HasColumnType("date");

        builder.Property(ri => ri.Certification)
            .HasMaxLength(255);

        builder.Property(ri => ri.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
