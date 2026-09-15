using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;

[Description("SmsMessages")]
public class SmsMessageDto
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "PhoneNumber")] public string PhoneNumber { get; set; } = null!;
    [Display(Name = "Message")] public string Message { get; set; } = null!;
    [Display(Name = "Direction")] public SmsDirection Direction { get; set; }
    [Display(Name = "SmsProvider")] public SmsProvider SmsProvider { get; set; }
    [Display(Name = "SmsStatus")] public SmsStatus SmsStatus { get; set; }
    [Display(Name = "ProviderMessageId")] public string? ProviderMessageId { get; set; }
    [Display(Name = "PartsCount")] public int PartsCount { get; set; }
    [Display(Name = "Encoding")] public string? Encoding { get; set; }
    [Display(Name = "ReceivedAt")] public DateTime? SMSDate { get; set; }
    [Display(Name = "DeliveredAt")] public DateTime? DeliveredAt { get; set; }
    [Display(Name = "ErrorMessage")] public string? ErrorMessage { get; set; }
    [Display(Name = "RetryCount")] public int RetryCount { get; set; }
}