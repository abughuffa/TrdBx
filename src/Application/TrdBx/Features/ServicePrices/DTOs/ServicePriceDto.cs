// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;

[Description("ServicePrices")]
public class ServicePriceDto
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
    [Description("Desc")]
    public string Desc { get; set; } = string.Empty;
    [Description("Price")]
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

