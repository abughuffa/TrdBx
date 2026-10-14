// using System.Net;
// using System.Net.Http.Json;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
// using CleanArchitecture.Blazor.Application.Common.Interfaces;
// using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
// using Microsoft.Extensions.Logging;

// namespace CleanArchitecture.Blazor.Infrastructure.Services;

// public class SmsGatewayClient : ISmsGatewayClient
// {
//     private readonly HttpClient _http;
//     private readonly ISmsGatewaySettingsProvider _provider;
//     private readonly ISmsGatewaySettingsCache _cache;
//     private readonly ILogger<SmsGatewayClient> _log;

//     public SmsGatewayClient(
//         HttpClient http,
//         ISmsGatewaySettingsProvider provider,
//         ISmsGatewaySettingsCache cache,
//         ILogger<SmsGatewayClient> log)
//     {
//         _http = http;
//         _provider = provider;
//         _cache = cache;
//         _log = log;
//     }

//     private async Task ApplyConfigAsync(CancellationToken ct)
//     {
//         // Prefer the cache; if empty (very first call after startup), use
//         // the provider to trigger a DB read + seed.
//         var s = _cache.Current ?? await _provider.GetAsync(ct);

//         var baseUri = new Uri(s.BaseUrl);
//         if (_http.BaseAddress != baseUri) _http.BaseAddress = baseUri;

//         var timeout = TimeSpan.FromSeconds(s.TimeoutSeconds);
//         if (_http.Timeout != timeout) _http.Timeout = timeout;

//         _http.DefaultRequestHeaders.Remove("X-Api-Key");
//         if (!string.IsNullOrWhiteSpace(s.ApiKey))
//             _http.DefaultRequestHeaders.Add("X-Api-Key", s.ApiKey);
//     }


//     private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
//     {
//         PropertyNameCaseInsensitive = true,
//         DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
//     };



//     public async Task<SmsGatewayStatusDto> GetStatusAsync(CancellationToken ct = default)
//     {
//         await ApplyConfigAsync(ct);
//         using var resp = await _http.GetAsync("/status", ct);
//         await EnsureSuccessAsync(resp, "/status", ct);
//         return await resp.Content.ReadFromJsonAsync<SmsGatewayStatusDto>(JsonOpts, ct)
//                ?? throw new InvalidOperationException("Empty /status response");
//     }


//     public async Task<SendSmsResult> SendAsync(
//         string to, string body, int simSlot, string? reference, CancellationToken ct = default)
//     {
//         await ApplyConfigAsync(ct);

//         var payload = new
//         {
//             to,
//             body,
//             simSlot,
//             reference = string.IsNullOrWhiteSpace(reference) ? null : reference,
//         };

//         using var resp = await _http.PostAsJsonAsync("/send", payload, JsonOpts, ct);

//         if (resp.StatusCode is HttpStatusCode.BadRequest
//                             or HttpStatusCode.BadGateway
//                             or HttpStatusCode.ServiceUnavailable)
//         {
//             var problem = await TryReadAsync<ErrorResponse>(resp, ct);
//             var msg = problem?.Error ?? $"Gateway returned {(int)resp.StatusCode}.";
//             return new SendSmsResult(reference ?? "", "failed", reference, msg);
//         }

//         await EnsureSuccessAsync(resp, "/send", ct);

//         var result = await resp.Content.ReadFromJsonAsync<SendSmsResult>(JsonOpts, ct);
//         return result ?? new SendSmsResult(reference ?? "", "queued", reference, null);
//     }

//     public async Task<IReadOnlyList<SmsInboxItemDto>> GetInboxAsync(
//         long sinceId = 0, int limit = 200, CancellationToken ct = default)
//     {
//         await ApplyConfigAsync(ct);

//         if (limit is < 1 or > 1000)
//             throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit must be 1..1000.");

//         var url = $"/inbox?since={sinceId}&limit={limit}";
//         using var resp = await _http.GetAsync(url, ct);
//         await EnsureSuccessAsync(resp, url, ct);

//         var env = await resp.Content.ReadFromJsonAsync<InboxEnvelope>(JsonOpts, ct);
//         return env?.Messages ?? (IReadOnlyList<SmsInboxItemDto>)Array.Empty<SmsInboxItemDto>();
//     }

//     public async Task SetWebhookAsync(string url, string? secret, CancellationToken ct = default)
//     {
//         await ApplyConfigAsync(ct);

//         if (string.IsNullOrWhiteSpace(url))
//             throw new ArgumentException("Webhook URL is required.", nameof(url));

//         using var resp = await _http.PostAsJsonAsync(
//             "/webhook", new { url, secret }, JsonOpts, ct);
//         await EnsureSuccessAsync(resp, "/webhook", ct);

//         _log.LogInformation("SMS webhook registered: {Url}", url);
//     }

//     public async Task ClearWebhookAsync(CancellationToken ct = default)
//     {
//         await ApplyConfigAsync(ct);
//         using var resp = await _http.DeleteAsync("/webhook", ct);
//         if (resp.StatusCode == HttpStatusCode.NotFound) return;
//         await EnsureSuccessAsync(resp, "DELETE /webhook", ct);
//     }

//     private async Task EnsureSuccessAsync(
//         HttpResponseMessage resp, string endpoint, CancellationToken ct)
//     {
//         if (resp.IsSuccessStatusCode) return;

//         string? detail = null;
//         try
//         {
//             var problem = await TryReadAsync<ErrorResponse>(resp, ct);
//             detail = problem?.Error ?? problem?.Detail;
//         }
//         catch { }

//         var msg = detail is null
//             ? $"SMS gateway {(int)resp.StatusCode} {resp.ReasonPhrase} for {endpoint}."
//             : $"SMS gateway {(int)resp.StatusCode} for {endpoint}: {detail}";

//         if (resp.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
//             throw new UnauthorizedAccessException(msg);

//         throw new HttpRequestException(msg, inner: null, statusCode: resp.StatusCode);
//     }

//     private static async Task<T?> TryReadAsync<T>(
//         HttpResponseMessage resp, CancellationToken ct) where T : class
//     {
//         try
//         {
//             var body = await resp.Content.ReadAsStringAsync(ct);
//             if (string.IsNullOrWhiteSpace(body)) return null;
//             return JsonSerializer.Deserialize<T>(body, JsonOpts);
//         }
//         catch { return null; }
//     }

//     private sealed record InboxEnvelope(List<SmsInboxItemDto> Messages);
//     private sealed record ErrorResponse(string? Error, string? Detail);
// }

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

/// <summary>
/// HTTP client for the physical SMS gateway device.
///
/// All failure modes are normalised into <see cref="SmsGatewayException"/> so
/// that <c>SmsGatewayExceptionHandler&lt;,&gt;</c> can produce user-friendly
/// results without leaking HttpClient / socket / JSON details up the stack.
///
/// Exception contract:
///   • Non-success HTTP status       → SmsGatewayException (with StatusCode + Operation)
///   • Transport failure (DNS/TLS)   → SmsGatewayException (wrapping HttpRequestException)
///   • HttpClient timeout            → SmsGatewayException (wrapping TaskCanceledException)
///   • Malformed JSON response       → SmsGatewayException (wrapping JsonException)
///   • Caller-initiated cancellation → OperationCanceledException (rethrown untouched)
///   • Business-level "failed" reply → SendSmsResult with Status="failed" (not an exception)
/// </summary>
public class SmsGatewayClient : ISmsGatewayClient
{
    private readonly HttpClient _http;
    private readonly ISmsGatewaySettingsProvider _provider;
    private readonly ISmsGatewaySettingsCache _cache;
    private readonly ILogger<SmsGatewayClient> _log;

    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

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

    // ─────────────────────────────────────────────────────────────────────────
    // Configuration
    // ─────────────────────────────────────────────────────────────────────────

    private async Task ApplyConfigAsync(CancellationToken ct)
    {
        // Prefer the cache; fall back to the provider (which also seeds it).
        var s = _cache.Current ?? await _provider.GetAsync(ct).ConfigureAwait(false);

        if (s is null)
        {
            throw new SmsGatewayException(
                "SMS gateway settings are not configured.",
                operation: "ApplyConfig");
        }

        if (string.IsNullOrWhiteSpace(s.BaseUrl))
        {
            throw new SmsGatewayException(
                "SMS gateway Base URL is empty.",
                operation: "ApplyConfig");
        }

        if (!Uri.TryCreate(s.BaseUrl, UriKind.Absolute, out var baseUri))
        {
            throw new SmsGatewayException(
                $"SMS gateway Base URL '{s.BaseUrl}' is not a valid absolute URI.",
                operation: "ApplyConfig");
        }

        if (_http.BaseAddress != baseUri)
            _http.BaseAddress = baseUri;

        var timeout = TimeSpan.FromSeconds(s.TimeoutSeconds > 0 ? s.TimeoutSeconds : 30);
        if (_http.Timeout != timeout)
            _http.Timeout = timeout;

        _http.DefaultRequestHeaders.Remove("X-Api-Key");
        if (!string.IsNullOrWhiteSpace(s.ApiKey))
            _http.DefaultRequestHeaders.Add("X-Api-Key", s.ApiKey);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Public API
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<SmsGatewayStatusDto> GetStatusAsync(CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct).ConfigureAwait(false);

        return await SendAsync<SmsGatewayStatusDto>(
            operation: "GetStatus",
            factory: () => _http.GetAsync("/status", ct),
            ct).ConfigureAwait(false);
    }

    public async Task<SendSmsResult> SendAsync(
        string to, string body, int simSlot, string? reference, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipient number is required.", nameof(to));
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Message body is required.", nameof(body));

        await ApplyConfigAsync(ct).ConfigureAwait(false);

        var payload = new
        {
            to,
            body,
            simSlot,
            reference = string.IsNullOrWhiteSpace(reference) ? null : reference,
        };

        try
        {
            using var resp = await _http
                .PostAsJsonAsync("/send", payload, JsonOpts, ct)
                .ConfigureAwait(false);

            // Business-level rejection: the gateway understood us but refused.
            // These are *not* transport errors — return a result, don't throw.
            if (resp.StatusCode is HttpStatusCode.BadRequest
                                or HttpStatusCode.BadGateway
                                or HttpStatusCode.ServiceUnavailable)
            {
                var problem = await TryReadAsync<ErrorResponse>(resp, ct).ConfigureAwait(false);
                var msg = problem?.Error
                       ?? problem?.Detail
                       ?? $"Gateway returned {(int)resp.StatusCode}.";

                _log.LogWarning(
                    "SMS /send rejected: status={Status} reference={Reference} detail={Detail}",
                    (int)resp.StatusCode, reference, msg);

                return new SendSmsResult(reference ?? string.Empty, "failed", reference, msg);
            }

            await EnsureSuccessAsync(resp, "/send", "SendSms", ct).ConfigureAwait(false);

            var result = await resp.Content
                .ReadFromJsonAsync<SendSmsResult>(JsonOpts, ct)
                .ConfigureAwait(false);

            return result ?? new SendSmsResult(reference ?? string.Empty, "queued", reference, null);
        }
        catch (SmsGatewayException) { throw; }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (HttpRequestException ex)
        {
            throw new SmsGatewayException(
                $"HTTP failure calling SMS gateway '/send': {ex.Message}",
                operation: "SendSms",
                statusCode: (int?)ex.StatusCode,
                innerException: ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new SmsGatewayException(
                "SMS gateway timed out sending the message.",
                operation: "SendSms",
                innerException: ex);
        }
        catch (JsonException ex)
        {
            throw new SmsGatewayException(
                "SMS gateway returned an invalid response for '/send'.",
                operation: "SendSms",
                innerException: ex);
        }
    }

    public async Task<IReadOnlyList<SmsInboxItemDto>> GetInboxAsync(
        long sinceId = 0, int limit = 200, CancellationToken ct = default)
    {
        if (limit is < 1 or > 1000)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit must be 1..1000.");

        await ApplyConfigAsync(ct).ConfigureAwait(false);

        var url = $"/inbox?since={sinceId}&limit={limit}";

        var env = await SendAsync<InboxEnvelope>(
            operation: "GetInbox",
            factory: () => _http.GetAsync(url, ct),
            ct).ConfigureAwait(false);

        return env?.Messages ?? (IReadOnlyList<SmsInboxItemDto>)Array.Empty<SmsInboxItemDto>();
    }

    public async Task SetWebhookAsync(string url, string? secret, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Webhook URL is required.", nameof(url));

        await ApplyConfigAsync(ct).ConfigureAwait(false);

        await SendAsync<object?>(
            operation: "SetWebhook",
            factory: () => _http.PostAsJsonAsync("/webhook", new { url, secret }, JsonOpts, ct),
            ct).ConfigureAwait(false);

        _log.LogInformation("SMS webhook registered: {Url}", url);
    }

    public async Task ClearWebhookAsync(CancellationToken ct = default)
    {
        await ApplyConfigAsync(ct).ConfigureAwait(false);

        try
        {
            using var resp = await _http.DeleteAsync("/webhook", ct).ConfigureAwait(false);

            // Idempotent delete: 404 means it's already gone.
            if (resp.StatusCode == HttpStatusCode.NotFound) return;

            await EnsureSuccessAsync(resp, "DELETE /webhook", "ClearWebhook", ct).ConfigureAwait(false);
        }
        catch (SmsGatewayException) { throw; }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (HttpRequestException ex)
        {
            throw new SmsGatewayException(
                $"HTTP failure calling SMS gateway 'DELETE /webhook': {ex.Message}",
                operation: "ClearWebhook",
                statusCode: (int?)ex.StatusCode,
                innerException: ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new SmsGatewayException(
                "SMS gateway timed out clearing the webhook.",
                operation: "ClearWebhook",
                innerException: ex);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Internal transport helper
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Sends a request via <paramref name="factory"/>, normalises every failure
    /// mode into <see cref="SmsGatewayException"/>, and deserialises the JSON
    /// body into <typeparamref name="T"/>.
    /// </summary>
    private async Task<T> SendAsync<T>(
        string operation,
        Func<Task<HttpResponseMessage>> factory,
        CancellationToken ct)
    {
        try
        {
            using var resp = await factory().ConfigureAwait(false);
            await EnsureSuccessAsync(resp, operation, operation, ct).ConfigureAwait(false);

            // For fire-and-forget operations (SetWebhook), the body may be empty.
            if (typeof(T) == typeof(object))
                return default!;

            var payload = await resp.Content
                .ReadFromJsonAsync<T>(JsonOpts, ct)
                .ConfigureAwait(false);

            return payload
                ?? throw new SmsGatewayException(
                    $"SMS gateway returned an empty body for '{operation}'.",
                    operation: operation,
                    statusCode: (int)resp.StatusCode);
        }
        catch (SmsGatewayException) { throw; }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (HttpRequestException ex)
        {
            throw new SmsGatewayException(
                $"HTTP failure calling SMS gateway '{operation}': {ex.Message}",
                operation: operation,
                statusCode: (int?)ex.StatusCode,
                innerException: ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new SmsGatewayException(
                $"SMS gateway timed out for '{operation}'.",
                operation: operation,
                innerException: ex);
        }
        catch (JsonException ex)
        {
            throw new SmsGatewayException(
                $"SMS gateway returned an invalid response for '{operation}'.",
                operation: operation,
                innerException: ex);
        }
    }

    private async Task EnsureSuccessAsync(
        HttpResponseMessage resp,
        string endpoint,
        string operation,
        CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;

        string? detail = null;
        try
        {
            var problem = await TryReadAsync<ErrorResponse>(resp, ct).ConfigureAwait(false);
            detail = problem?.Error ?? problem?.Detail;
        }
        catch (Exception ex)
        {
            _log.LogDebug(ex,
                "Could not read problem-details body for {Operation}.", operation);
        }

        var msg = detail is null
            ? $"SMS gateway returned {(int)resp.StatusCode} for {endpoint}."
            : $"SMS gateway returned {(int)resp.StatusCode} for {endpoint}: {detail}";

        // 401/403 is a *configuration* problem worth surfacing distinctly.
        // The handler will map StatusCode → "credentials rejected" anyway,
        // but we still carry the code so logs and messages stay accurate.
        throw new SmsGatewayException(
            msg,
            operation: operation,
            statusCode: (int)resp.StatusCode);
    }

    private static async Task<T?> TryReadAsync<T>(
        HttpResponseMessage resp, CancellationToken ct) where T : class
    {
        try
        {
            var body = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(body)) return null;
            return JsonSerializer.Deserialize<T>(body, JsonOpts);
        }
        catch (JsonException)
        {
            // Body wasn't our expected error shape — caller will fall back
            // to the generic HTTP status message.
            return null;
        }
        catch (Exception)
        {
            // Cancellation / disposal races — treat as "no detail available".
            return null;
        }
    }

    private sealed record InboxEnvelope(List<SmsInboxItemDto> Messages);
    private sealed record ErrorResponse(string? Error, string? Detail);
}