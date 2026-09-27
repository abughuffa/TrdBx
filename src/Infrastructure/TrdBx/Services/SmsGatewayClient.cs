using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Blazor.Infrastructure.TrdBx.Services;

/// <summary>
/// HTTP client for the Android SMS Gateway.
/// Matches the gateway's contract:
///   GET    /status
///   POST   /send
///   GET    /inbox?since=&amp;limit=
///   POST   /webhook
///   DELETE /webhook
/// </summary>
public class SmsGatewayClient : ISmsGatewayClient
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly HttpClient _http;
    private readonly SmsGatewayOptions _opt;
    private readonly ILogger<SmsGatewayClient> _log;

    public SmsGatewayClient(
        HttpClient http,
        IOptions<SmsGatewayOptions> opt,
        ILogger<SmsGatewayClient> log)
    {
        _http = http;
        _opt  = opt.Value;
        _log  = log;

        _http.BaseAddress = new Uri(_opt.BaseUrl);
        _http.Timeout     = TimeSpan.FromSeconds(_opt.TimeoutSeconds);

        if (!string.IsNullOrWhiteSpace(_opt.ApiKey))
            _http.DefaultRequestHeaders.Add("X-Api-Key", _opt.ApiKey);
    }

    // ────────────────────────────────────────────────────────────────
    // 1. GET /status
    // ────────────────────────────────────────────────────────────────
    public async Task<SmsGatewayStatusDto> GetStatusAsync(CancellationToken ct = default)
    {
        using var resp = await _http.GetAsync("/status", ct);
        await EnsureSuccessAsync(resp, "/status", ct);

        var dto = await resp.Content
            .ReadFromJsonAsync<SmsGatewayStatusDto>(JsonOpts, ct);

        if (dto is null)
            throw new InvalidOperationException(
                "SMS gateway returned an empty body for GET /status.");

        return dto;
    }

    // ────────────────────────────────────────────────────────────────
    // 2. POST /send
    // ────────────────────────────────────────────────────────────────
    public async Task<SendSmsResult> SendAsync(
        string to,
        string body,
        int simSlot,
        string? reference,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipient is required.", nameof(to));
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Message body is required.", nameof(body));

        var payload = new
        {
            to,
            body,
            simSlot,
            reference = string.IsNullOrWhiteSpace(reference) ? null : reference,
        };

        using var resp = await _http.PostAsJsonAsync("/send", payload, JsonOpts, ct);

        // Gateway returns 202 Accepted on success, 502 Bad Gateway on failure,
        // 400 Bad Request on validation errors.
        if (resp.StatusCode is HttpStatusCode.BadRequest)
        {
            var problem = await TryReadAsync<ErrorResponse>(resp, ct);
            var msg = problem?.Error ?? "Bad request to SMS gateway.";
            _log.LogWarning("Gateway rejected /send: {Error}", msg);
            return new SendSmsResult(
                Id:         reference ?? string.Empty,
                Status:     "failed",
                Reference:  reference,
                Error:      msg);
        }

        if (resp.StatusCode is HttpStatusCode.BadGateway
                            or HttpStatusCode.ServiceUnavailable)
        {
            var problem = await TryReadAsync<ErrorResponse>(resp, ct);
            var msg = problem?.Error ?? $"Gateway returned {(int)resp.StatusCode}.";
            _log.LogWarning("Gateway /send failed: {Error}", msg);
            return new SendSmsResult(
                Id:         reference ?? string.Empty,
                Status:     "failed",
                Reference:  reference,
                Error:      msg);
        }

        await EnsureSuccessAsync(resp, "/send", ct);

        var result = await resp.Content
            .ReadFromJsonAsync<SendSmsResult>(JsonOpts, ct);

        if (result is null)
        {
            _log.LogWarning("Gateway returned empty body for /send; treating as queued.");
            return new SendSmsResult(
                Id:         reference ?? string.Empty,
                Status:     "queued",
                Reference:  reference,
                Error:      null);
        }

        if (!string.IsNullOrWhiteSpace(result.Error))
            _log.LogWarning("Gateway reported send error: {Error}", result.Error);

        return result;
    }

    // ────────────────────────────────────────────────────────────────
    // 3. GET /inbox?since=&limit=
    // ────────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<SmsInboxItemDto>> GetInboxAsync(
        long sinceId = 0,
        int limit = 200,
        CancellationToken ct = default)
    {
        if (limit is < 1 or > 1000)
            throw new ArgumentOutOfRangeException(
                nameof(limit), limit, "limit must be between 1 and 1000.");

        var url = $"/inbox?since={sinceId}&limit={limit}";

        using var resp = await _http.GetAsync(url, ct);
        await EnsureSuccessAsync(resp, url, ct);

        var env = await resp.Content
            .ReadFromJsonAsync<InboxEnvelope>(JsonOpts, ct);

        return env?.Messages ?? (IReadOnlyList<SmsInboxItemDto>)Array.Empty<SmsInboxItemDto>();
    }

    // ────────────────────────────────────────────────────────────────
    // 4. POST /webhook
    // ────────────────────────────────────────────────────────────────
    public async Task SetWebhookAsync(
        string url,
        string? secret,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Webhook URL is required.", nameof(url));
        if (!Uri.TryCreate(url, UriKind.Absolute, out var parsed)
            || (parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                $"Webhook URL must be an absolute http/https URL: '{url}'.", nameof(url));
        }

        var payload = new
        {
            url,
            secret = string.IsNullOrWhiteSpace(secret) ? null : secret,
        };

        using var resp = await _http.PostAsJsonAsync("/webhook", payload, JsonOpts, ct);
        await EnsureSuccessAsync(resp, "/webhook", ct);

        _log.LogInformation(
            "Registered SMS webhook {Url} (secret {HasSecret})",
            url, string.IsNullOrWhiteSpace(secret) ? "no" : "yes");
    }

    // ────────────────────────────────────────────────────────────────
    // 5. DELETE /webhook
    // ────────────────────────────────────────────────────────────────
    public async Task ClearWebhookAsync(CancellationToken ct = default)
    {
        using var resp = await _http.DeleteAsync("/webhook", ct);

        // 204 No Content and 200 OK are both "success" here. Some gateways
        // return 404 if there is nothing to remove — treat that as success too.
        if (resp.StatusCode == HttpStatusCode.NotFound)
        {
            _log.LogDebug("Cleared SMS webhook; gateway reported nothing to remove.");
            return;
        }

        await EnsureSuccessAsync(resp, "DELETE /webhook", ct);
        _log.LogInformation("Cleared SMS webhook.");
    }

    // ────────────────────────────────────────────────────────────────
    // Helpers
    // ────────────────────────────────────────────────────────────────
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
        catch { /* ignore body parse errors */ }

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
        catch
        {
            return null;
        }
    }

    private sealed record InboxEnvelope(List<SmsInboxItemDto> Messages);

    private sealed record ErrorResponse(string? Error, string? Detail);
}