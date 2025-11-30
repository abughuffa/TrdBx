// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Enums;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.DTOs;

[Description("WialonUnits")]
public class WialonUnitDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string UnitName { get; set; } = string.Empty;
    [Description("Account")]
    public string? Account { get; set; }
    [Description("UID")]
    public string? UnitSNo { get; set; }
    [Description("Phone")]
    public string? SimCardNo { get; set; }

    [Description("Deactivation")]
    public DateTime? Deactivation { get; set; }


    // Calculated from Deactivation column where if it has a value StatusOnWialon will be Inactive otherwise equals Active
    [Description("StatusOnWialon")]
    public WStatus StatusOnWialon { get; set; }
    [Description("Note")]
    public string? Note { get; set; } = string.Empty;




    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<WialonUnit, WialonUnitDto>(MemberList.None);
    //        CreateMap<WialonUnitDto, WialonUnit>(MemberList.None);
    //    }
    //}





}

