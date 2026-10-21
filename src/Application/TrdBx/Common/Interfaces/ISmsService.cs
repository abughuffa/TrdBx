

using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface ISmsSender
{
    SmsProvider SmsProvider { get; }

    Task<SmsSendResult> SendAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default);
}


public interface ISmsReceiver// NEW: separate abstraction so not every provider must implement receiving
{
    SmsProvider SmsProvider { get; }
    IAsyncEnumerable<SmsReceiveResult> ListenAsync(CancellationToken cancellationToken);
}

public record SmsReceiveResult(
    string PhoneNumber,
    string Message,
    DateTime SMSDate,
    string? ProviderMessageId = null,
    int PartsCount = 1,
    string? Encoding = null);

public record SmsSendResult(
    bool Success,
    string? ProviderMessageId = null,
    string? ErrorMessage = null,
    int PartsCount = 1,
    string? Encoding = null);