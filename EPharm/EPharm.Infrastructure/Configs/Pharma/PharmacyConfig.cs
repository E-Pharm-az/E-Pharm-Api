using EPharm.Infrastructure.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Pharma;

public class PharmacyConfig : IEntityTypeConfiguration<Pharmacy>
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
        
        builder.HasMany(pc => pc.PharmacyStaff)
            .WithOne(pcm => pcm.Pharmacy)
            .HasForeignKey(pcm => pcm.PharmacyId);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
