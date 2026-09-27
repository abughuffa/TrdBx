
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface ISmsGatewayClient
{
    Task<SmsGatewayStatusDto> GetStatusAsync(CancellationToken ct = default);

    Task<SendSmsResult> SendAsync(
        string to,
        string body,
        int simSlot,
        string? reference,
        CancellationToken ct = default);

    Task<IReadOnlyList<SmsInboxItemDto>> GetInboxAsync(
        long sinceId = 0,
        int limit = 200,
        CancellationToken ct = default);

    Task SetWebhookAsync(string url, string? secret, CancellationToken ct = default);

    Task ClearWebhookAsync(CancellationToken ct = default);
}