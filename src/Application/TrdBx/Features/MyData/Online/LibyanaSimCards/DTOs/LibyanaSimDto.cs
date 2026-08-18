// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.DTOs;

[Description("LibyanaSimCards")]
public class LibyanaSimCardDto
{
    [Display(Name ="Id")]
    public int Id { get; set; }
    [Display(Name ="Service Number")]
    public string? SimCardNo { get; set; }
    //[Description("State")]
    //public string? SimCardStatus { get; set; }
    [Display(Name ="State")]
    public SLStatus? SimCardStatus { get; set; }
    [Display(Name ="Main Bal")]
    public decimal? Balance { get; set; }
    [Display(Name ="Main Bal Expiry Date")]
    public DateTime? BExDate { get; set; }
    [Display(Name ="Join Date")]
    public DateTime? JoinDate { get; set; }
    [Display(Name ="Product Name")]
    public string? Package { get; set; }
    [Display(Name ="Data Expiry Date")]
    public DateTime? DExDate { get; set; }
    [Display(Name ="Data Offer")]
    public string? DataOffer { get; set; }
    [Display(Name ="Data Offer Expired")]
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

