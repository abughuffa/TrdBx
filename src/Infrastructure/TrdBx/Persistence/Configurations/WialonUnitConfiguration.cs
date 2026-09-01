using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable

public class WialonUnitConfiguration : IEntityTypeConfiguration<WialonUnit>
{
    public void Configure(EntityTypeBuilder<WialonUnit> builder)
    {

            builder.ToTable("WialonUnits");
            builder.Ignore(e => e.DomainEvents);

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
            .UseIdentityColumn();

            builder.Property(e => e.UnitName)
            .HasMaxLength(100)
            .HasColumnName("UnitName");

            builder.Property(e => e.Account)
            .HasMaxLength(50)
            .HasColumnName("Account");

            builder.Property(e => e.UnitSNo)
            .HasMaxLength(50)
            .HasColumnName("UnitSNo");

            builder.Property(e => e.Deactivation)
            .HasColumnName("Deactivation");

            builder.Property(e => e.SimCardNo)
            .HasMaxLength(50)
            .HasColumnName("SimCardNo");

            builder.Property(e => e.StatusOnWialon)
            // .HasConversion<string>()
            .HasColumnName("StatusOnWialon");

            builder.Property(e => e.Note)
            .HasMaxLength(2000)
            .HasColumnName("Note");
        
        // Indexes

            builder.HasIndex(e => e.UnitSNo)
                .HasDatabaseName("IX_WialonUnit_UnitSNo");
            
            builder.HasIndex(e => e.SimCardNo)
                .HasDatabaseName("IX_WialonUnit_SimCardNo");

    }
}