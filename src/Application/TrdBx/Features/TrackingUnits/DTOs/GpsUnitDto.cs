using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

[Description("TrackingUnits")]
public class TrackingUnitDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("SNo")] public  string SNo { get; set; } = string.Empty;
    [Description("Imei")] public string? Imei { get; set; }
    [Description("UnitName")]  public string? UnitName { get; set; }
    [Description("TrackingUnitModelId")] public int TrackingUnitModelId { get; set; }
    [Description("UStatus")] public UStatus UStatus { get; set; }
    [Description("InsMode")] public InsMode InsMode { get; set; }
    [Description("WryDate")] public DateOnly? WryDate { get; set; }
    [Description("TrackedAssetId")] public int? TrackedAssetId { get; set; }
    [Description("SimCardId")] public int? SimCardId { get; set; }
    [Description("CustomerId")] public int? CustomerId { get; set; }
    [Description("IsOnWialon")] public bool IsOnWialon { get; set; }
    [Description("WStatus")] public WStatus WStatus { get; set; }
    [Description("WUnitId")] public int? WUnitId { get; set; }
    [Description("OldId")] public int? OldId { get; set; }


    //public TrackingUnitModelDto? TrackingUnitModelDto { get; set; }
    [Description("TrackingUnitModel")] public string? TrackingUnitModel { get; set; }
    [Description("Customer")] public string? Customer { get; set; }
    [Description("SimCard")] public string? SimCard { get; set; }
    [Description("TrackedAsset")] public string? TrackedAsset { get; set; }





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

