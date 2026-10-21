// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;

[Description("ServicePrices")]
public class ServicePriceDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }

   [Display(Name = "ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
   [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
   [Display(Name = "Price")]
    public decimal Price { get; set; } = 0.0m;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<ServicePrice, ServicePriceDto>(MemberList.None);
    //        CreateMap<ServicePriceDto, ServicePrice>(MemberList.None);
    //    }
    //}
}

