
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class SmsGatewaySettingsConfiguration : IEntityTypeConfiguration<SmsGatewaySettings>
{
    public void Configure(EntityTypeBuilder<SmsGatewaySettings> builder)
    {
        builder.ToTable("SmsGatewaySettings");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);

        
        builder.Property(x => x.BaseUrl)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("BaseUrl"); 

       builder.Property(x => x.ApiKey)
            .HasMaxLength(128)
            .HasColumnName("ApiKey");

          builder.Property(x => x.WebhookPublicUrl)
            .HasMaxLength(128)
            .HasColumnName("WebhookPublicUrl");
          builder.Property(x => x.WebhookPublicUrl)
            .HasMaxLength(128)
            .HasColumnName("WebhookPublicUrl");
          builder.Property(x => x.WebhookSecret)
            .HasMaxLength(128)
            .HasColumnName("WebhookSecret");
    }
}