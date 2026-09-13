using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class SmsMessageConfiguration : IEntityTypeConfiguration<SmsMessage>
{
    public void Configure(EntityTypeBuilder<SmsMessage> builder)
    {
        builder.ToTable("SmsMessages");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("PhoneNumber");

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1600) // 10 concatenated parts
            .HasColumnName("Message");

        builder.Property(x => x.SmsProvider)
            .HasConversion<int>()
            .IsRequired()
            .HasColumnName("SmsProvider");

        builder.Property(x => x.SmsStatus)
            .HasConversion<int>()
            .IsRequired()
            .HasColumnName("SmsStatus");

        builder.Property(x => x.ProviderMessageId)
            .HasMaxLength(100)
            .HasColumnName("ProviderMessageId");

        builder.Property(x => x.Encoding)
            .HasMaxLength(20)
            .HasColumnName("Encoding");

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1000)
            .HasColumnName("ErrorMessage");

        // builder.Property(x => x.RelatedEntityType)
        //     .HasMaxLength(100)
        //     .HasColumnName("RelatedEntityType");

        // builder.Property(x => x.SentByUserId)
        //     .HasMaxLength(450)
        //     .HasColumnName("SentByUserId");

        // Indexes for common queries
        builder.HasIndex(x => x.PhoneNumber);
        builder.HasIndex(x => x.SmsStatus);
        builder.HasIndex(x => x.SmsProvider);
        // builder.HasIndex(x => x.Created);
        // builder.HasIndex(x => new { x.RelatedEntityType, x.RelatedEntityId });
        builder.HasIndex(x => x.ProviderMessageId);
    }
}