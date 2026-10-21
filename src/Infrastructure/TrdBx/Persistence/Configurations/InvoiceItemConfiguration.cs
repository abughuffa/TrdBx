using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;
public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {

        builder.ToTable("InvoiceItems");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);
     
        builder.Property(e => e.Id)
        .UseIdentityColumn();

            builder.Property(e => e.SubSerialIndex)
            .IsRequired()
            .HasColumnName("SubSerialIndex");

            builder.Property(e => e.InvoiceItemGroupId)
            .IsRequired()
            .HasColumnName("InvoiceItemGroupId");

            builder.Property(e => e.SubscriptionId)
            .IsRequired()
            .HasColumnName("SubscriptionId");

            builder.Property(e => e.Description)
            .HasMaxLength(256)
            .HasColumnName("Description");
            
            builder.Property(e => e.Amount)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("Amount");
            
            // Indexes
            builder.HasIndex(e => e.InvoiceItemGroupId)
                .HasDatabaseName("IX_InvoiceItem_InvoiceItemGroupId");
            
            builder.HasIndex(e => e.SubscriptionId)
                .HasDatabaseName("IX_InvoiceItem_SubscriptionId");
            

            // Relationships
            builder.HasOne(e => e.InvoiceItemGroup)
                .WithMany(e => e.InvoiceItems)
                .HasForeignKey(e => e.InvoiceItemGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(e => e.Subscription)
                .WithOne(e => e.InvoiceItem)
                .HasForeignKey<InvoiceItem>(e => e.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}

