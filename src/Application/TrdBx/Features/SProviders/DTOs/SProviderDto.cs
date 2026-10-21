// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.DTOs;

[Description("SProviders")]
public class SProviderDto
{
    [Display(Name = "Id")]
    public int Id { get; set; } = 0;
    [Display(Name = "Name")]
    public string? Name { get; set; }


    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SProvider, SProviderDto>(MemberList.None);
    //        CreateMap<SProviderDto, SProvider>(MemberList.None);
    //    }
    //}

}

