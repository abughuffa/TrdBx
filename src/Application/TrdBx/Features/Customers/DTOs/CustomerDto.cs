using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.DTOs;

[Description("Customers")]
public class CustomerDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ParentId")]
    public int? ParentId { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Account")]
    public string Account { get; set; } = string.Empty;
    [Description("UserName")]
    public string UserName { get; set; } = string.Empty;

    [Description("BillingPlan")]
    public BillingPlan BillingPlan { get; set; }
    [Description("IsTaxable")]
    public bool IsTaxable { get; set; } = false;
    [Description("IsRenewable")]
    public bool IsRenewable { get; set; } = false;

    [Description("WUserId")]
    public int? WUserId { get; set; }

    [Description("WUnitGroupId")]
    public int? WUnitGroupId { get; set; }
    [Description("Address")]
    public string? Address { get; set; } = string.Empty;
    [Description("Mobile1")]
    public string? Mobile1 { get; set; } = string.Empty;
    [Description("Mobile2")]
    public string? Mobile2 { get; set; } = string.Empty;
    [Description("Email")]
    public string? Email { get; set; } = string.Empty;
    [Description("IsAvaliable")]
    public bool IsAvaliable { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;


    [Description("Parent")] public string? Parent { get; set; }

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

