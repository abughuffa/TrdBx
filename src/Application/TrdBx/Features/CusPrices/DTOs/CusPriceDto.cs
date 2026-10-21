#nullable enable
#nullable disable warnings

using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
[Description("CusPrices")]
public class CusPriceDto
{
     [Display(Name ="Id")]
    public int Id { get; set; }


     [Display(Name ="CustomerId")]
    public int CustomerId { get; set; }

    [Display(Name ="TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }
    [Display(Name ="Host")]
    public decimal Host { get; set; }
    [Display(Name ="Gprs")]
    public decimal Gprs { get; set; }
    [Display(Name ="Price")]
    public decimal Price { get; set; }

    [Display(Name ="Customer")]
    public string? Customer { get; set; }

    [Display(Name ="TrackingUnitModel")]
    public string? TrackingUnitModel { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CusPrice, CusPriceDto>(MemberList.None)
    //            .ForMember(dest => dest.Customer,
    //                  opt => opt.MapFrom(src => (src.Customer == null ? null : src.Customer.Name)))
    //            .ForMember(dest => dest.TrackingUnitModel,
    //                  opt => opt.MapFrom(src => (src.TrackingUnitModel == null ? null : src.TrackingUnitModel.Name)));

    //        CreateMap<CusPriceDto, CusPrice>(MemberList.None);
    //    }
    //}

}

