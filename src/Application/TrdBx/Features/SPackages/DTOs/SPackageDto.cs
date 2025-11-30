// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;

[Description("SPackages")]
public class SPackageDto
{
    [Description("Id")]
    public int Id { get; set; } = 0;
    [Description("Name")]
    public string? Name { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SPackage, SPackageDto>(MemberList.None);
    //        CreateMap<SPackageDto, SPackage>(MemberList.None);
    //    }
    //}

}

