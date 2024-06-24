using EPharm.Infrastructure.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Pharma;

public class PharmacyStaffConfig : IEntityTypeConfiguration<PharmacyStaff>
{
    public void Configure(EntityTypeBuilder<PharmacyStaff> builder)
    {
        builder.Property(pcm => pcm.ExternalId)
            .IsRequired();

        builder.Property(pcm => pcm.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(pcm => pcm.Pharmacy)
            .WithMany(pc => pc.PharmacyStaff)
            .HasForeignKey(pcm => pcm.PharmacyId);

        builder.Property(pcm => pcm.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}