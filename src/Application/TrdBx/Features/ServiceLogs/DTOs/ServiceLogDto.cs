using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;

[Description("ServiceLogs")]
public class ServiceLogDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ServiceNo")]
    public string ServiceNo { get; set; } = string.Empty;
    [Description("ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("InstallerId")]
    public  string InstallerId { get; set; } = string.Empty;
    [Description("Desc")]
    public string Desc { get; set; } = string.Empty;
    [Description("SerDate")]
    public DateOnly SerDate { get; set; }
    [Description("IsDeserved")]
    public bool IsDeserved { get; set; } = true;
    [Description("IsBilled")]
    public bool IsBilled { get; set; } = false;
    [Description("Amount")]
    public decimal Amount { get; set; } = 0.0m;


    [Description("Customer")] public string? Customer { get; set; }
    [Description("Installer")] public string? Installer { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<ServiceLog, ServiceLogDto>(MemberList.None)
    //            .ForMember(dest => dest.Customer,
    //                  opt => opt.MapFrom(src => (src.Customer == null ? null : src.Customer.Name)))
    //                .ForMember(dest => dest.Installer,
    //                  opt => opt.MapFrom(src => (src.Installer == null ? null : src.Installer.DisplayName)));
    //        CreateMap<ServiceLogDto, ServiceLog>(MemberList.None);
    //    }
    //}
}

