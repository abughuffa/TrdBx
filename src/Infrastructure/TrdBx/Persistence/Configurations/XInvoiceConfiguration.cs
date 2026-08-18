using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasIndex(t => t.InvoiceNo).IsUnique(true);
        builder.Property(t => t.InvoiceNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.Ignore(e => e.DomainEvents);
    }
}


