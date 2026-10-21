// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.DTOs;

[Description("WialonUnits")]
public class WialonUnitDto
{
    [Display(Name ="Id")]
    public int Id { get; set; }
    [Display(Name ="Name")]
    public string UnitName { get; set; } = string.Empty;
    [Display(Name ="Account")]
    public string? Account { get; set; }
    [Display(Name ="UID")]
    public string? UnitSNo { get; set; }
    [Display(Name ="Phone")]
    public string? SimCardNo { get; set; }

    [Display(Name ="Deactivation")]
    public DateTime? Deactivation { get; set; }


    // Calculated from Deactivation column where if it has a value StatusOnWialon will be Inactive otherwise equals Active
    [Display(Name ="StatusOnWialon")]
    public WStatus? StatusOnWialon { get; set; }
    [Display(Name ="Note")]
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

