using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;
public class InvoiceItemsGroupConfiguration : IEntityTypeConfiguration<InvoiceItemGroup>
{
    public void Configure(EntityTypeBuilder<InvoiceItemGroup> builder)
    {

        builder.ToTable("InvoiceItemGroups");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);
     
        builder.Property(e => e.Id)
        .UseIdentityColumn();

            builder.Property(e => e.SerialIndex)
            .IsRequired()
            .HasColumnName("SerialIndex");

            builder.Property(e => e.InvoiceId)
            .IsRequired()
            .HasColumnName("InvoiceId");

            builder.Property(e => e.ServiceLogId)
            .IsRequired()
            .HasColumnName("ServiceLogId");

            builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("Description");

            builder.Property(e => e.Amount)
            .IsRequired().HasPrecision(9, 3)
            .HasColumnName("Amount");
            
            builder.Property(e => e.SubTotal)
            .IsRequired().HasPrecision(9, 3)
            .HasColumnName("SubTotal");
            
            // Indexes

            builder.HasIndex(e => e.InvoiceId)
                .HasDatabaseName("IX_InvoiceItemGroup_InvoiceId");
            
            builder.HasIndex(e => e.ServiceLogId)
                .HasDatabaseName("IX_InvoiceItemGroup_ServiceLogId");


             // Relationships

            builder.HasOne(e => e.Invoice)
                .WithMany(e => e.InvoiceItemGroups)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(e => e.ServiceLog)
                .WithOne(e => e.InvoiceItemGroup)
                .HasForeignKey<InvoiceItemGroup>(e => e.ServiceLogId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(e => e.InvoiceItems)
                .WithOne(e => e.InvoiceItemGroup)
                .HasForeignKey(e => e.InvoiceItemGroupId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}

