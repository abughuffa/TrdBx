using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class SmsInboundPartConfiguration : IEntityTypeConfiguration<SmsInboundPart>
{
    public void Configure(EntityTypeBuilder<SmsInboundPart> builder)
    {
        builder.ToTable("SmsInboundParts");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);



        builder.Property(x => x.From)
        .HasMaxLength(32)
        .HasColumnName("From")
        .IsRequired();

        builder.Property(x => x.Body)
        .HasColumnName("Body")
        .IsRequired();
        

        builder.HasIndex(x => new { x.From, x.ConcatRef, x.PartNumber, x.TotalParts })
         .IsUnique()
         .HasDatabaseName("UX_SmsInboundPart_Exact");

        // Fast group lookup.
        builder.HasIndex(x => new { x.From, x.ConcatRef, x.AssembledSmsMessageId })
         .HasDatabaseName("IX_SmsInboundPart_Group");

        builder.HasOne(x => x.AssembledSmsMessage)
         .WithMany()
         .HasForeignKey(x => x.AssembledSmsMessageId)
         .OnDelete(DeleteBehavior.SetNull);

    }
}