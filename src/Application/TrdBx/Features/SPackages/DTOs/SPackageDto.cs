// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;

[Description("SPackages")]
public class SPackageDto
{
    [Display(Name = "Id")]
    public int Id { get; set; } = 0;
    [Display(Name = "Name")]

    public string? Name { get; set; }
     [Display(Name = "SProviderId")]
    public int SProviderId { get; set; }

  [Display(Name = "OldId")]
    public int? OldId { get; set; } = null;

   [Display(Name = "SProvider")] public string? SProvider { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SPackage, SPackageDto>(MemberList.None);
    //        CreateMap<SPackageDto, SPackage>(MemberList.None);
    //    }
    //}

}

