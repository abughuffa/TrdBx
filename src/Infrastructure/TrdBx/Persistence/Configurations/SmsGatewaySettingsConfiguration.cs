
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
        .HasColumnName("BaseUrl")
        .HasMaxLength(500).IsRequired();
        builder.Property(x => x.ApiKey)
        .HasColumnName("ApiKey")
        .HasMaxLength(500);

        // Existing defaults for pre-existing installs.
        builder.Property(x => x.EnableInboundPolling)
        .HasColumnName("EnableInboundPolling")
        .HasDefaultValue(true);
        builder.Property(x => x.PollingIntervalSeconds)
        .HasColumnName("PollingIntervalSeconds")
        .HasDefaultValue(30);
        builder.Property(x => x.TimeoutSeconds)
        .HasColumnName("TimeoutSeconds")
        .HasDefaultValue(30);

        // Sweeper defaults — mirror the DTO/entity initializers.
        builder.Property(x => x.SweeperIntervalSeconds)
        .HasColumnName("SweeperIntervalSeconds")
        .HasDefaultValue(60);
        builder.Property(x => x.AssemblyWindowMinutes)
        .HasColumnName("AssemblyWindowMinutes")
        .HasDefaultValue(10);
        builder.Property(x => x.OrphanThresholdMinutes)
        .HasColumnName("OrphanThresholdMinutes")
        .HasDefaultValue(30);
        builder.Property(x => x.RetentionDays)
        .HasColumnName("RetentionDays")
        .HasDefaultValue(30);
        builder.Property(x => x.MaxGroupsPerSweep)
        .HasColumnName("MaxGroupsPerSweep")
        .HasDefaultValue(50);
    }
}