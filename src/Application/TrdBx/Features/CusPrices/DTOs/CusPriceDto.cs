using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
[Description("CusPrices")]
public class CusPriceDto
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("CustomerId")]
    public int CustomerId { get; set; }

    [Description("TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }
    [Description("Host")]
    public decimal Host { get; set; }
    [Description("Gprs")]
    public decimal Gprs { get; set; }
    [Description("Price")]
    public decimal Price { get; set; }

    [Description("Customer")]  public string? Customer { get; set; }

    [Description("TrackingUnitModel")] public string? TrackingUnitModel { get; set; }

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

