using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.DTOs;

[Description("SmsConfigTemplates")]
public class SmsConfigTemplateDto
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TrackingUnitModelId")] public int TrackingUnitModelId { get; set; }
    [Display(Name = "Name")] public  string Name { get; set; } = string.Empty;
    [Display(Name = "SmsTemplate")] public string SmsTemplate { get; set; } = string.Empty;
    [Display(Name = "TrackingUnitModel")] public string? TrackingUnitModel { get; set; }



}

