using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class CusPriceConfiguration : IEntityTypeConfiguration<CusPrice>
// {
//     public void Configure(EntityTypeBuilder<CusPrice> builder)
//     {
//         builder.Property(t => t.CustomerId).IsRequired();
//         builder.Property(t => t.TrackingUnitModelId).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }

public class CusPriceConfiguration : IEntityTypeConfiguration<CusPrice>
{
    public void Configure(EntityTypeBuilder<CusPrice> builder)
    {
        builder.ToTable("CusPrices");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId");

        builder.Property(e => e.TrackingUnitModelId)
            .IsRequired()
            .HasColumnName("TrackingUnitModelId");
        
        builder.Property(e => e.Host)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("Host");
        
        builder.Property(e => e.Gprs)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("Gprs");
        
        builder.Property(e => e.Price)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("Price");

        // Indexes
        
        builder.HasIndex(e => e.CustomerId)
            .HasDatabaseName("IX_CusPrice_CustomerId");
        
        builder.HasIndex(e => e.TrackingUnitModelId)
            .HasDatabaseName("IX_CusPrice_TrackingUnitModelId");
        
        // Relationships
        
        builder.HasOne(e => e.Customer)
            .WithMany(e => e.CusPrices)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TrackingUnitModel)
            .WithMany(e => e.CusPrices)
            .HasForeignKey(e => e.TrackingUnitModelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


