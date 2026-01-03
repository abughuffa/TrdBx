// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.DTOs;

[Description("LibyanaSimCards")]
public class LibyanaSimCardDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Service Number")]
    public string? SimCardNo { get; set; }
    //[Description("State")]
    //public string? SimCardStatus { get; set; }
    [Description("State")]
    public SLStatus? SimCardStatus { get; set; }
    [Description("Main Bal")]
    public decimal? Balance { get; set; }
    [Description("Main Bal Expiry Date")]
    public DateTime? BExDate { get; set; }
    [Description("Join Date")]
    public DateTime? JoinDate { get; set; }
    [Description("Product Name")]
    public string? Package { get; set; }
    [Description("Data Expiry Date")]
    public DateTime? DExDate { get; set; }
    [Description("Data Offer")]
    public string? DataOffer { get; set; }
    [Description("Data Offer Expired")]
    public DateTime? DOExpired { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<LibyanaSimCard, LibyanaSimCardDto>(MemberList.None);
    //        CreateMap<LibyanaSimCardDto, LibyanaSimCard>(MemberList.None);
    //    }
    //}
}

