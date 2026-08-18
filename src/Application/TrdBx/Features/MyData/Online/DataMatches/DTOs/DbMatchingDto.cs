// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.DTOs;

[Description("DataMatches")]
public class DataMatchDto
{

    [Display(Name ="Account")]
    public string? Account { get; set; }
    [Display(Name ="Client")]
    public string? Client { get; set; }
    [Display(Name ="Customer")]
    public string? Customer { get; set; }

    [Display(Name ="WUnitSNo")]
    public string? WUnitSNo { get; set; }
    [Display(Name ="TUnitSNo")]
    public string? TUnitSNo { get; set; }
    [Display(Name ="WSimCardNo")]
    public string? WSimCardNo { get; set; }
    [Display(Name ="TSimCardNo")]
    public string? TSimCardNo { get; set; }

    [Display(Name ="StatusOnWialon")]
    public WStatus StatusOnWialon { get; set; }

    [Display(Name ="StatusOnTrdBx")]
    public UStatus StatusOnTrdBx { get; set; }

    //[Display(Name ="TNote")]
    //public string? TNote { get; set; }

    [Display(Name ="WNote")]
    public string? WNote { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<DataMatch, DataMatchDto>(MemberList.None);
    //        CreateMap<DataMatchDto, DataMatch>(MemberList.None);
    //    }
    //}
}


