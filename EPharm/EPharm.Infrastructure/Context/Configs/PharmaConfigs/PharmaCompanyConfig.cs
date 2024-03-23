using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.PharmaConfigs;

public class PharmaCompanyConfig : IEntityTypeConfiguration<PharmaCompany>
{
    public void Configure(EntityTypeBuilder<PharmaCompany> builder)
    {
        builder.Property(pc => pc.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.TIN)
            .IsRequired();

        builder.Property(pc => pc.ContactEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.ContactPhone)
            .HasMaxLength(20);
        
        builder.Property(pc => pc.StreetAddress)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(pc => pc.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(pc => pc.City)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(pc => pc.Country)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(pc => pc.Region)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasMany(pc => pc.PharmaCompanyManagers)
            .WithOne(pcm => pcm.PharmaCompany)
            .HasForeignKey(pcm => pcm.PharmaCompanyId);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
