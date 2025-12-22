// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{

    [DisplayName("Shipment Permissions")]
    [Description("Set permissions for Shipment operations.")]
    public static class Shipments
    {
        public const string View = "Permissions.Shipments.View";  //trader & transporter permission
        public const string Create = "Permissions.Shipments.Create"; //trader permission
        public const string Edit = "Permissions.Shipments.Edit"; //trader permission
        public const string Delete = "Permissions.Shipments.Delete"; //trader permission
        public const string Print = "Permissions.Shipments.Print"; //trader permission
        public const string Search = "Permissions.Shipments.Search"; //trader & transporter permission

        public const string BidOnOrder = "Permissions.Shipments.BidOnOrder"; //transporter permission
        public const string Assign = "Permissions.Shipments.Assign";  //trader permission
        public const string Accept = "Permissions.Shipments.Accept";  //transporter permission

    }
}

public static partial class Permissions
{

    [DisplayName("Vehicle Permissions")]
    [Description("Set permissions for Vehicle operations.")]
    public static class Vehicles
    {
        public const string View = "Permissions.Vehicles.View"; //trader & transporter permission
        public const string Create = "Permissions.Vehicles.Create"; //transporter permission
        public const string Edit = "Permissions.Vehicles.Edit"; //transporter permission
        public const string Delete = "Permissions.Vehicles.Delete"; //transporter permission
        public const string Print = "Permissions.Vehicles.Print"; //transporter permission
        public const string Search = "Permissions.Vehicles.Search"; //transporter permission
    }

}

public static partial class Permissions
{

    [DisplayName("BidRecord Permissions")]
    [Description("Set permissions for BidRecord operations.")]
    public static class BidRecords
    {
        public const string View = "Permissions.BidRecords.View"; //trader & transporter permission
        public const string Create = "Permissions.BidRecords.Create"; //transporter permission
        public const string Edit = "Permissions.BidRecords.Edit"; //transporter permission
        public const string Delete = "Permissions.BidRecords.Delete"; //transporter permission
        public const string Print = "Permissions.BidRecords.Print"; //transporter permission
        public const string Search = "Permissions.BidRecords.Search"; //transporter permission
    }

}

public static partial class Permissions
{

    [DisplayName("POI Permissions")]
    [Description("Set permissions for POI operations.")]
    public static class POIs
    { 
        public const string View = "Permissions.POIs.View"; //trader permission
    public const string Create = "Permissions.POIs.Create"; //trader permission
    public const string Edit = "Permissions.POIs.Edit"; //trader permission
    public const string Delete = "Permissions.POIs.Delete"; //trader permission
    public const string Print = "Permissions.POIs.Print"; //trader permission
    public const string Search = "Permissions.POIs.Search"; //trader permission
}
}


public static partial class Permissions
{

    [DisplayName("VehicleType Permissions")]
    [Description("Set permissions for VehicleType operations.")]
    public static class VehicleTypes
    {
        public const string View = "Permissions.VehicleTypes.View"; //admin permission
        public const string Create = "Permissions.VehicleTypes.Create"; //admin permission
        public const string Edit = "Permissions.VehicleTypes.Edit"; //admin permission
        public const string Delete = "Permissions.VehicleTypes.Delete"; //admin permission
        public const string Print = "Permissions.VehicleTypes.Print"; //admin permission
        public const string Search = "Permissions.VehicleTypes.Search"; //admin permission
    }

}
