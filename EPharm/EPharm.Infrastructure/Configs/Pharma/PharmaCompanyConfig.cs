using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.PharmaConfigs;

public class PharmaCompanyConfig : IEntityTypeConfiguration<Pharmacy>
{
    public void Configure(EntityTypeBuilder<Pharmacy> builder)
    {
        builder.Property(pc => pc.Name)
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
            .HasMaxLength(255);
        
        builder.Property(pc => pc.PostalCode)
            .HasMaxLength(20);
        
        builder.Property(pc => pc.City)
            .HasMaxLength(255);
        
        builder.Property(pc => pc.Country)
            .HasMaxLength(255);
        
        builder.Property(pc => pc.Region)
            .HasMaxLength(255);
        
        builder.HasMany(pc => pc.Managers)
            .WithOne(pcm => pcm.Pharmacy)
            .HasForeignKey(pcm => pcm.PharmaCompanyId);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
