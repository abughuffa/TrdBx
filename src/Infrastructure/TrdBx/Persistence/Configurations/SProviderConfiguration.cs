using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class SProviderConfiguration : IEntityTypeConfiguration<SProvider>
// {
//     public void Configure(EntityTypeBuilder<SProvider> builder)
//     {
//         builder.HasIndex(t => t.Name).IsUnique(true);
//         builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }



public class SProviderConfiguration : IEntityTypeConfiguration<SProvider>
{
    public void Configure(EntityTypeBuilder<SProvider> builder)
    {
        builder.ToTable("SProviders");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Name");

        // Indexes
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_SProvider_Name");
        
        // Relationships
        builder.HasMany(e => e.SPackages)
            .WithOne(e => e.SProvider)
            .HasForeignKey(e => e.SProviderId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}


