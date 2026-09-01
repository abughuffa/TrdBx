using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class CcConfiguration : IEntityTypeConfiguration<Customer>
// {
//     public void Configure(EntityTypeBuilder<Customer> builder)
//     {
        
//         builder.HasIndex(t => t.Name).IsUnique(true);
//         builder.Property(t => t.Name).HasMaxLength(256).IsRequired();
//         builder.Property(t => t.Account).HasMaxLength(256).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }


public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.ParentId)
            .HasColumnName("ParentId");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");
        
        builder.Property(e => e.Account)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Account");
        
        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("UserName");
        
        builder.Property(e => e.BillingPlan)
            .IsRequired()
            //.HasConversion<string>()
            .HasColumnName("BillingPlan");
        
        builder.Property(e => e.IsTaxable)
            .IsRequired()
            .HasColumnName("IsTaxable");
        
        builder.Property(e => e.IsRenewable)
            .IsRequired()
            .HasColumnName("IsRenewable");
        
        builder.Property(e => e.WUserId)
            .HasColumnName("WUserId");
        
        builder.Property(e => e.WUnitGroupId)
            .HasColumnName("WUnitGroupId");
        
        builder.Property(e => e.Address)
            .HasMaxLength(500)
            .HasColumnName("Address");
        
        builder.Property(e => e.Mobile1)
            .HasMaxLength(50)
            .HasColumnName("Mobile1");
        
        builder.Property(e => e.Mobile2)
            .HasMaxLength(50)
            .HasColumnName("Mobile2");
        
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .HasColumnName("Email");
        
        builder.Property(e => e.IsAvailable)
            .IsRequired()
            .HasColumnName("IsAvailable");
        
        builder.Property(e => e.OldId)
            .HasColumnName("OldId");
        
        // Indexes
        builder.HasIndex(e => e.Account)
            // .IsUnique()
            .HasDatabaseName("IX_Customer_Account");
        
        builder.HasIndex(e => e.UserName)
            // .IsUnique()
            .HasDatabaseName("IX_Customer_UserName");
        
        builder.HasIndex(e => e.Email)
            .HasDatabaseName("IX_Customer_Email");
        
        builder.HasIndex(e => e.ParentId)
            .HasDatabaseName("IX_Customer_ParentId");
        
        // Relationships
        builder.HasOne(e => e.Parent)
            .WithMany(e => e.Childs)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.Invoices)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.CusPrices)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.ServiceLogs)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

