using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.DTOs;

[Description("Customers")]
public class CustomerDto
{
    [Display(Name ="Id")]
    public int Id { get; set; }
     [Display(Name ="ParentId")]
    public int? ParentId { get; set; }
    [Display(Name ="Name")]
    public string Name { get; set; } = string.Empty;
    [Display(Name ="Account")]
    public string Account { get; set; } = string.Empty;
    [Display(Name ="UserName")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name ="BillingPlan")]
    public BillingPlan BillingPlan { get; set; }
    [Display(Name ="IsTaxable")]
    public bool IsTaxable { get; set; } = false;
    [Display(Name ="IsRenewable")]
    public bool IsRenewable { get; set; } = false;

    [Display(Name ="WUserId")]
    public int? WUserId { get; set; }

    [Display(Name ="WUnitGroupId")]
    public int? WUnitGroupId { get; set; }
    [Display(Name ="Address")]
    public string? Address { get; set; } = string.Empty;
    [Display(Name ="Mobile1")]
    public string? Mobile1 { get; set; } = string.Empty;
    [Display(Name ="Mobile2")]
    public string? Mobile2 { get; set; } = string.Empty;
    [Display(Name ="Email")]
    public string? Email { get; set; } = string.Empty;
    [Display(Name ="IsAvailable")]
    public bool IsAvailable { get; set; }
    [Display(Name ="OldId")]
    public int? OldId { get; set; } = null;


    [Display(Name ="Parent")] public string? Parent { get; set; }


    [Display(Name ="ParentChild")] public string? ParentChild => Parent is null ? Name : $"{Parent} - {Name}";




    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<Customer, CustomerDto>(MemberList.None)
    //            .ForMember(dest => dest.Parent,
    //                  opt => opt.MapFrom(src => (src.Parent == null ? null : src.Parent.Name)));

    //        //CreateMap<Customer, CustomerDto>(MemberList.None)
    //        //   .ForMember(dest => dest.Parent,
    //        //         opt => opt.MapFrom(src => src.Parent?.Name));

    //        CreateMap<CustomerDto, Customer>(MemberList.None);
    //    }
    //}

}



