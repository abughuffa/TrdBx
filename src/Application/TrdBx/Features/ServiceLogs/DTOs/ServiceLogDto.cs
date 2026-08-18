using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.Identity.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;

[Description("ServiceLogs")]
public class ServiceLogDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name ="ServiceNo")]
    public string ServiceNo { get; set; } = string.Empty;
    [Display(Name = "ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
    [Display(Name = "CustomerId")]
    public int CustomerId { get; set; }
    //[Description("InstallerId")]
    //public  string InstallerId { get; set; } = string.Empty;
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "SerDate")]
    public DateOnly SerDate { get; set; }
    [Description("IsDeserved")]
    public bool IsDeserved { get; set; } = true;
    [Display(Name = "IsBilled")]
    public bool IsBilled { get; set; } = false;
    [Display(Name = "Amount")]
    public decimal Amount { get; set; } = 0.0m;


    [Display(Name = "Customer")] public string? Customer { get; set; }
    [Display(Name = "Created By User")] public ApplicationUserDto? CreatedByUser { get; set; }

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

