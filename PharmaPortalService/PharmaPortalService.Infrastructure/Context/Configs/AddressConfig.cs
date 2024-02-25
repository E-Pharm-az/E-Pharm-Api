using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities;

namespace PharmaPortalService.Infrastructure.Context.Configs;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.StreetAddress)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Region)
            .HasMaxLength(100);
        
        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
