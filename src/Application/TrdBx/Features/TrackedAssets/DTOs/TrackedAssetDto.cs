using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;

[Description("TrackedAssets")]
public class TrackedAssetDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TrackedAssetNo")]
    public string? TrackedAssetNo { get; set; }
    [Description("TrackedAssetCode")]
    public string? TrackedAssetCode { get; set; }
    [Description("VinSerNo")]
    public string? VinSerNo { get; set; }
    [Description("PlateNo")]
    public string? PlateNo { get; set; }
    [Description("TrackedAssetDesc")]
    public string? TrackedAssetDesc { get; set; }

    [Description("IsAvaliable")]
    public bool IsAvaliable { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;
    [Description("OldVehicleNo")]
    public string? OldVehicleNo { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackedAsset, TrackedAssetDto>(MemberList.None);
    //        CreateMap<TrackedAssetDto, TrackedAsset>(MemberList.None);
    //    }
    //}

}

