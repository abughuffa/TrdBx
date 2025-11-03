using CleanArchitecture.Blazor.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class ActiveTestCaseConfiguration : IEntityTypeConfiguration<ActivateTestCase>
{
    public void Configure(EntityTypeBuilder<ActivateTestCase> builder)
    {
        builder.Property(t => t.SNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


