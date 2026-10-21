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

        builder.Property(x => x.To)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("To");
        
        builder.Property(x => x.From)
            .HasMaxLength(20)
            .HasColumnName("From");

        builder.Property(x => x.Body)
            .IsRequired()
            .HasMaxLength(1600) // 10 concatenated parts
            .HasColumnName("Body");
        
        builder.Property(x => x.SimSlot)
            .HasConversion<int>()
            .IsRequired()
            .HasColumnName("SimSlot");

        builder.Property(x => x.Reference)
            .HasMaxLength(512)
            .HasColumnName("Reference");

        builder.Property(x => x.GatewayId)
            .HasMaxLength(512)
            .HasColumnName("GatewayId");

        builder.Property(x => x.Direction)
            .HasConversion<int>()
            .IsRequired()
            .HasColumnName("Direction");

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired()
            .HasColumnName("Status");

        builder.Property(x => x.Error)
            .HasMaxLength(1024)
            .HasColumnName("Error");

        builder.Property(e => e.SentAt)
            .HasColumnName("SentAt");

        builder.Property(e => e.DeliveredAt)
            .HasColumnName("DeliveredAt");

        builder.Property(e => e.ReceivedAt)
            .HasColumnName("ReceivedAt");

        builder.Property(x => x.AssemblyKey)
                    .HasColumnName("AssemblyKey")
                    .HasMaxLength(200);
        builder.Property(x => x.PartsGatewayIds)
                .HasColumnName("PartsGatewayIds")
                .HasMaxLength(2000);


        // Indexes for common queries
        // builder.HasIndex(x => x.Direction);
        // builder.HasIndex(x => x.Status);

        // builder.HasIndex(x => x.GatewayId)
        //     .IsUnique()
        //     .HasFilter("\"GatewayId\" IS NOT NULL AND \"Direction\" = 1"); 


              // Single-part dedupe: unchanged.
        builder.HasIndex(x => new { x.Direction, x.GatewayId })
         .IsUnique()
        .HasFilter("\"GatewayId\" IS NOT NULL");

        // Multipart assembly claim: at most one SmsMessage per
        // (From, ConcatRef, TotalParts). Filtered so NULL never collides.
        builder.HasIndex(x => x.AssemblyKey)
         .IsUnique()
         .HasFilter("\"AssemblyKey\" IS NOT NULL")
         .HasDatabaseName("UX_SmsMessage_AssemblyKey");


    }
}