using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

[Description("TrackingUnits")]
public class TrackingUnitDto
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "SNo")] public  string SNo { get; set; } = string.Empty;
    [Display(Name = "Imei")] public string? Imei { get; set; }
    [Display(Name = "UnitName")]  public string? UnitName { get; set; }
    [Display(Name = "TrackingUnitModelId")] public int TrackingUnitModelId { get; set; }
    [Display(Name = "UStatus")] public UStatus UStatus { get; set; }
    [Display(Name = "InsMode")] public InsMode InsMode { get; set; }
    [Display(Name = "WryDate")] public DateOnly? WryDate { get; set; }
    [Display(Name = "TrackedAssetId")] public int? TrackedAssetId { get; set; }
    [Display(Name = "SimCardId")] public int? SimCardId { get; set; }
    [Display(Name = "CustomerId")] public int? CustomerId { get; set; }
    [Display(Name = "IsOnWialon")] public bool IsOnWialon { get; set; }
    [Display(Name = "WStatus")] public WStatus WStatus { get; set; }
    [Display(Name = "WunitId")] public int? WUnitId { get; set; }
    [Display(Name = "OldId")] public int? OldId { get; set; }


    //public TrackingUnitModelDto? TrackingUnitModelDto { get; set; }
    [Display(Name = "TrackingUnitModel")] public string? TrackingUnitModel { get; set; }
    [Display(Name = "Customer")] public string? Customer { get; set; }
    [Display(Name = "SimCard")] public string? SimCard { get; set; }
    [Display(Name = "TrackedAsset")] public string? TrackedAsset { get; set; }





    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnit, TrackingUnitDto>(MemberList.None)
    //         .ForMember(dest => dest.TrackingUnitModel,
    //                  opt => opt.MapFrom(src => (src.TrackingUnitModel == null ? null : src.TrackingUnitModel.Name)))
    //         .ForMember(dest => dest.Customer,
    //                  opt => opt.MapFrom(src => (src.Customer == null ? null : src.Customer.Name)))
    //        .ForMember(dest => dest.SimCard,
    //                  opt => opt.MapFrom(src => (src.SimCard == null ? null : src.SimCard.SimCardNo)))
    //        .ForMember(dest => dest.TrackedAsset,
    //                  opt => opt.MapFrom(src => (src.TrackedAsset == null ? null : src.TrackedAsset.TrackedAssetNo)));

    //        CreateMap<TrackingUnitDto, TrackingUnit>(MemberList.None);
    //    }
    //}


}

