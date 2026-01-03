// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.DTOs;

[Description("DataMatches")]
public class DataMatchDto
{

    [Description("Account")]
    public string? Account { get; set; }
    [Description("Client")]
    public string? Client { get; set; }
    [Description("Customer")]
    public string? Customer { get; set; }

    [Description("WUnitSNo")]
    public string? WUnitSNo { get; set; }
    [Description("TUnitSNo")]
    public string? TUnitSNo { get; set; }
    [Description("WSimCardNo")]
    public string? WSimCardNo { get; set; }
    [Description("TSimCardNo")]
    public string? TSimCardNo { get; set; }

    [Description("StatusOnWialon")]
    public WStatus StatusOnWialon { get; set; }

    [Description("StatusOnTrdBx")]
    public UStatus StatusOnTrdBx { get; set; }

    [Description("TNote")]
    public string? TNote { get; set; }

    [Description("WNote")]
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


