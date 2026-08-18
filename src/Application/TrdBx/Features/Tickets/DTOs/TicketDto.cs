using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.Identity.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;

[Description("Tickets")]
public class TicketDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "TicketNo")]
    public string TicketNo { get; set; } = string.Empty;
    [Display(Name = "ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "TicketStatus")]
    public TicketStatus TicketStatus { get; set; }
    [Display(Name = "TrackingUnitId")]
    public int TrackingUnitId { get; set; }

    [Display(Name = "TcDate")]
    public DateOnly TcDate { get; set; }

    [Display(Name = "TaDate")]
    public DateOnly? TaDate { get; set; }

    //[Display(Name = "InstallerId")]
    //public string? InstallerId { get; set; }
    [Display(Name = "TeDate")]
    public DateOnly? TeDate { get; set; }
    [Display(Name = "Note")]
    public string? Note { get; set; } = string.Empty;


    [Display(Name = "TrackingUnit")] public string? TrackingUnit { get; set; }


    [Display(Name = "Created By User")] public ApplicationUserDto? CreatedByUser { get; set; }

    [Display(Name = "Modified By User")] public virtual ApplicationUserDto? LastModifiedByUser { get; set; }
    //[Display(Name = "Installer")] public string? Installer { get; set; }
    //private class Mapping : Profile
    //{
    //    //public Mapping()
    //    //{
    //    //    CreateMap<Ticket, TicketDto>(MemberList.None)
    //    //        .ForMember(dest => dest.TrackingUnit,
    //    //              opt => opt.MapFrom(src => (src.TrackingUnit == null ? null : src.TrackingUnit.SNo)))
    //    //                 .ForMember(dest => dest.Installer,
    //    //              opt => opt.MapFrom(src => (src.Installer == null ? null : src.Installer.DisplayName)));

    //    //    CreateMap<TicketDto, Ticket>(MemberList.None);
    //    //}
    //}

}

