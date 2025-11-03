using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class CcConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        
        builder.HasIndex(t => t.Name).IsUnique(true);
        builder.Property(t => t.Name).HasMaxLength(256).IsRequired();
        builder.Property(t => t.Account).HasMaxLength(256).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


