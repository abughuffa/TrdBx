using CleanArchitecture.Blazor.Domain.Common.Entities;
using System.ComponentModel;

namespace CleanArchitecture.Blazor.Domain.Entities;


public class DeactivateTestCase : BaseEntity
{
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }

    [Description("InstallerId")]
    public required string InstallerId { get; set; }

    [Description("SNo")]
    public string? SNo { get; set; }

    [Description("TsDate")]
    public DateOnly TsDate { get; set; }

    [Description("CaseCode")]
    public int? CaseCode { get; set; }

    [Description("IsSucssed")]
    public bool? IsSucssed { get; set; }

    [Description("Message")]
    public string? Message { get; set; }
}

public class ActivateTestCase : BaseEntity
{
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }

    [Description("InstallerId")]
    public required string InstallerId { get; set; }

    [Description("SNo")]
    public string? SNo { get; set; }

    [Description("TsDate")]
    public DateOnly TsDate { get; set; }
    [Description("CaseCode")]
    public int? CaseCode { get; set; }

    [Description("IsSucssed")]
    public bool? IsSucssed { get; set; }

    [Description("Message")]
    public string? Message { get; set; }
}

public class ActivateGprsTestCase : BaseEntity
{
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }

    [Description("InstallerId")]
    public required string InstallerId { get; set; }

    [Description("SNo")]
    public string? SNo { get; set; }

    [Description("TsDate")]
    public DateOnly TsDate { get; set; }
    [Description("CaseCode")]
    public int? CaseCode { get; set; }

    [Description("IsSucssed")]
    public bool? IsSucssed { get; set; }

    [Description("Message")]
    public string? Message { get; set; }
}

public class ActivateHostingTestCase : BaseEntity
{
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }

    [Description("InstallerId")]
    public required string InstallerId { get; set; }

    [Description("SNo")]
    public string? SNo { get; set; }

    [Description("TsDate")]
    public DateOnly TsDate { get; set; }
    [Description("CaseCode")]
    public int? CaseCode { get; set; }

    [Description("IsSucssed")]
    public bool? IsSucssed { get; set; }

    [Description("Message")]
    public string? Message { get; set; }
}
