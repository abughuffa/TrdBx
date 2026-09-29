using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class SmsGatewayClient : ISmsGatewayClient
{
    private readonly HttpClient _http;
    private readonly ISmsGatewaySettingsProvider _provider;
    private readonly ISmsGatewaySettingsCache _cache;
    private readonly ILogger<SmsGatewayClient> _log;

    public SmsGatewayClient(
        HttpClient http,
        ISmsGatewaySettingsProvider provider,
        ISmsGatewaySettingsCache cache,
        ILogger<SmsGatewayClient> log)
    {
        _http = http;
        _provider = provider;
        _cache = cache;
        _log = log;
    }

    private async Task ApplyConfigAsync(CancellationToken ct)
    {
        // Prefer the cache; if empty (very first call after startup), use
        // the provider to trigger a DB read + seed.
        var s = _cache.Current ?? await _provider.GetAsync(ct);

        var baseUri = new Uri(s.BaseUrl);
        if (_http.BaseAddress != baseUri) _http.BaseAddress = baseUri;

        var timeout = TimeSpan.FromSeconds(s.TimeoutSeconds);
        if (_http.Timeout != timeout) _http.Timeout = timeout;

        _http.DefaultRequestHeaders.Remove("X-Api-Key");
        if (!string.IsNullOrWhiteSpace(s.ApiKey))
            _http.DefaultRequestHeaders.Add("X-Api-Key", s.ApiKey);
    }


    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };



    public async Task<SmsGatewayStatusDto> GetStatusAsync(CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct);
        using var resp = await _http.GetAsync("/status", ct);
        await EnsureSuccessAsync(resp, "/status", ct);
        return await resp.Content.ReadFromJsonAsync<SmsGatewayStatusDto>(JsonOpts, ct)
               ?? throw new InvalidOperationException("Empty /status response");
    }

    public async Task<SendSmsResult> SendAsync(
        string to, string body, int simSlot, string? reference, CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct);

        var payload = new
        {
            to,
            body,
            simSlot,
            reference = string.IsNullOrWhiteSpace(reference) ? null : reference,
        };

        using var resp = await _http.PostAsJsonAsync("/send", payload, JsonOpts, ct);

        if (resp.StatusCode is HttpStatusCode.BadRequest
                            or HttpStatusCode.BadGateway
                            or HttpStatusCode.ServiceUnavailable)
        {
            var problem = await TryReadAsync<ErrorResponse>(resp, ct);
            var msg = problem?.Error ?? $"Gateway returned {(int)resp.StatusCode}.";
            return new SendSmsResult(reference ?? "", "failed", reference, msg);
        }

        await EnsureSuccessAsync(resp, "/send", ct);

        var result = await resp.Content.ReadFromJsonAsync<SendSmsResult>(JsonOpts, ct);
        return result ?? new SendSmsResult(reference ?? "", "queued", reference, null);
    }

    public async Task<IReadOnlyList<SmsInboxItemDto>> GetInboxAsync(
        long sinceId = 0, int limit = 200, CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct);

        if (limit is < 1 or > 1000)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit must be 1..1000.");

        var url = $"/inbox?since={sinceId}&limit={limit}";
        using var resp = await _http.GetAsync(url, ct);
        await EnsureSuccessAsync(resp, url, ct);

        var env = await resp.Content.ReadFromJsonAsync<InboxEnvelope>(JsonOpts, ct);
        return env?.Messages ?? (IReadOnlyList<SmsInboxItemDto>)Array.Empty<SmsInboxItemDto>();
    }

    public async Task SetWebhookAsync(string url, string? secret, CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct);

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Webhook URL is required.", nameof(url));

        using var resp = await _http.PostAsJsonAsync(
            "/webhook", new { url, secret }, JsonOpts, ct);
        await EnsureSuccessAsync(resp, "/webhook", ct);

        _log.LogInformation("SMS webhook registered: {Url}", url);
    }

    public async Task ClearWebhookAsync(CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct);
        using var resp = await _http.DeleteAsync("/webhook", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return;
        await EnsureSuccessAsync(resp, "DELETE /webhook", ct);
    }

    private async Task EnsureSuccessAsync(
        HttpResponseMessage resp, string endpoint, CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;

        string? detail = null;
        try
        {
            var problem = await TryReadAsync<ErrorResponse>(resp, ct);
            detail = problem?.Error ?? problem?.Detail;
        }
        catch { }

        var msg = detail is null
            ? $"SMS gateway {(int)resp.StatusCode} {resp.ReasonPhrase} for {endpoint}."
            : $"SMS gateway {(int)resp.StatusCode} for {endpoint}: {detail}";

        if (resp.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException(msg);

        throw new HttpRequestException(msg, inner: null, statusCode: resp.StatusCode);
    }

    private static async Task<T?> TryReadAsync<T>(
        HttpResponseMessage resp, CancellationToken ct) where T : class
    {
        try
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            if (string.IsNullOrWhiteSpace(body)) return null;
            return JsonSerializer.Deserialize<T>(body, JsonOpts);
        }
        catch { return null; }
    }

    private sealed record InboxEnvelope(List<SmsInboxItemDto> Messages);
    private sealed record ErrorResponse(string? Error, string? Detail);
}