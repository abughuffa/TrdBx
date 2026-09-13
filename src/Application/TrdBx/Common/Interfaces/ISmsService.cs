

using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface ISmsService
{
    SmsProvider SmsProvider { get; }

    Task<SmsSendResult> SendAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default);
}

public record SmsSendResult(
    
    bool Success,
    string? ProviderMessageId = null,
    string? ErrorMessage = null,
    int PartsCount = 1,
    string? Encoding = null);