using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;

[Description("TrackedAssets")]
public class TrackedAssetDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "TrackedAssetNo")]
    public string? TrackedAssetNo { get; set; }
    [Display(Name = "TrackedAssetCode")]
    public string? TrackedAssetCode { get; set; }
    [Display(Name = "VinSerNo")]
    public string? VinSerNo { get; set; }
    [Display(Name = "PlateNo")]
    public string? PlateNo { get; set; }
    [Display(Name = "TrackedAssetDesc")]
    public string? TrackedAssetDesc { get; set; }

    [Display(Name = "IsAvaliable")]
    public bool IsAvaliable { get; set; }
    [Display(Name = "OldId")]
    public int? OldId { get; set; } = null;
    [Display(Name = "OldVehicleNo")]
    public string? OldVehicleNo { get; set; } = null;


    
    public List<TrackingUnitDto>? TrackingUnits { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackedAsset, TrackedAssetDto>(MemberList.None);
    //        CreateMap<TrackedAssetDto, TrackedAsset>(MemberList.None);
    //    }
    //}

}

