using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class SmsCursorConfiguration : IEntityTypeConfiguration<SmsCursor>
{
    public void Configure(EntityTypeBuilder<SmsCursor> builder)
    {
        builder.ToTable("SmsCursors");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);

        
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("Key"); 
        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Value");
    }
}