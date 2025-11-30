using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class SPackageConfiguration : IEntityTypeConfiguration<SPackage>
{
    public void Configure(EntityTypeBuilder<SPackage> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique(true);
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


