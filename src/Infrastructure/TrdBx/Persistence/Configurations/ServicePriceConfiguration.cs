using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class SPackageConfiguration : IEntityTypeConfiguration<SPackage>
// {
//     public void Configure(EntityTypeBuilder<SPackage> builder)
//     {
//         builder.HasIndex(t => t.Name).IsUnique(true);
//         builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }

public class ServicePriceConfiguration : IEntityTypeConfiguration<ServicePrice>
{
    public void Configure(EntityTypeBuilder<ServicePrice> builder)
    {
        builder.ToTable("ServicePrices");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.ServiceTask)
        .IsRequired()
        // .HasConversion<string>()
        .HasColumnName("ServiceTask");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("Description");

        builder.Property(e => e.Price)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("Price");

        // Indexes
    
        builder.HasIndex(e => e.ServiceTask)
                .HasDatabaseName("IX_ServicePrice_ServiceTask");
        
    }
}


