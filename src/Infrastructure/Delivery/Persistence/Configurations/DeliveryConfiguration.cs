// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable




public class ShipmentVehicleTypeConfiguration : IEntityTypeConfiguration<ShipmentVehicleType>
{
    public void Configure(EntityTypeBuilder<ShipmentVehicleType> builder)
    {
        builder.HasKey(x => new { x.ShipmentId, x.VehicleTypeId });

        builder
            .HasOne(x => x.Shipment)
            .WithMany(x => x.VehicleTypes)
            .HasForeignKey(x => x.ShipmentId);

        builder
            .HasOne(x => x.VehicleType)
            .WithMany(x => x.Shipments)
            .HasForeignKey(x => x.VehicleTypeId);
    }
}

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.Property(t => t.ShipmentNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(t => t.VehicleNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

