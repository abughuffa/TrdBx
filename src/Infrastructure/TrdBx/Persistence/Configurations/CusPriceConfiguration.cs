using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class CusPriceConfiguration : IEntityTypeConfiguration<CusPrice>
{
    public void Configure(EntityTypeBuilder<CusPrice> builder)
    {
        builder.Property(t => t.CustomerId).IsRequired();
        builder.Property(t => t.TrackingUnitModelId).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


