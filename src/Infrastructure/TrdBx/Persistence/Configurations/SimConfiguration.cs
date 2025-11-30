using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class SimCardConfiguration : IEntityTypeConfiguration<SimCard>
{
    public void Configure(EntityTypeBuilder<SimCard> builder)
    {
        builder.HasIndex(t => t.SimCardNo).IsUnique(true);
        builder.Property(t => t.SimCardNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


