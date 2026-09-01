using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable

public class LibyanaSimCardConfiguration : IEntityTypeConfiguration<LibyanaSimCard>
{
    public void Configure(EntityTypeBuilder<LibyanaSimCard> builder)
    {
            builder.ToTable("LibyanaSimCards");
            builder.Ignore(e => e.DomainEvents);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
            .UseIdentityColumn();



            builder.Property(e => e.SimCardNo)
            .HasMaxLength(50)
            .HasColumnName("SimCardNo");

            builder.Property(e => e.SimCardStatus)
            // .HasConversion<string>()
            .HasColumnName("SimCardStatus");

            builder.Property(e => e.Balance)
            .HasPrecision(9, 3)
            .HasColumnName("Balance");

            builder.Property(e => e.BExDate)
            .HasColumnName("BExDate");

            builder.Property(e => e.JoinDate)
            .HasColumnName("JoinDate");

            builder.Property(e => e.Package)
            .HasMaxLength(200)
            .HasColumnName("Package");

            builder.Property(e => e.DExDate)
            .HasColumnName("DExDate");

            builder.Property(e => e.DataOffer)
            .HasMaxLength(200)
            .HasColumnName("DataOffer");

            builder.Property(e => e.DOExpired)
            .HasColumnName("DOExpired");
            
            // Indexes

            builder.HasIndex(e => e.SimCardNo)
                .HasDatabaseName("IX_LibyanaSimCard_SimCardNo");

    }
}