// CleanArchitecture.Blazor.Application/Common/ExceptionHandlers/SmsGatewayExceptionHandler.cs
using System.Text.Json;

namespace CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

/// <summary>
/// Converts SMS-gateway failures into <see cref="IResult"/> failures so that
/// command/query handlers can surface friendly messages to the UI instead of
/// bubbling raw HTTP/socket errors up to the Blazor error boundary.
///
/// Catches:
///   • <see cref="SmsGatewayException"/> — domain-specific wrapper thrown by
///     <c>ISmsGatewayClient</c> implementations.
///   • <see cref="HttpRequestException"/> — transport-level failures
///     (DNS, refused connection, TLS, 5xx when not wrapped).
///   • <see cref="TaskCanceledException"/> — HttpClient timeout (caller NOT
///     cancelled).
///   • <see cref="JsonException"/> — malformed gateway payloads.
/// </summary>
public sealed class SmsGatewayExceptionHandler<TRequest, TResponse>
    : MessageExceptionHandler<TRequest, TResponse, Exception>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<SmsGatewayExceptionHandler<TRequest, TResponse>> _logger;

    public SmsGatewayExceptionHandler(
        ILogger<SmsGatewayExceptionHandler<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(
        TRequest request,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // ── Re-throw genuine caller-initiated cancellation ────────────────────
        // MessageExceptionHandler<TRequest,TResponse,Exception> will match ANY
        // exception, so we must explicitly let OperationCanceledException
        // triggered by the caller's token escape — otherwise cancellation
        // (page navigation, request abort, shutdown) is swallowed and the
        // result object pretends everything is fine.
        if (exception is OperationCanceledException && cancellationToken.IsCancellationRequested)
        {
            throw exception;
        }

        // ── Anything that is not a gateway problem → let Fallback handle it ───
        // Returning Unhandled means: this handler declines, and the pipeline
        // gives the next matching handler a chance (FallbackExceptionHandler).
        if (!IsGatewayException(exception))
        {
            //return Unhandled();
            throw exception;
        }

        var (userMessage, logLevel) = MapToUserMessage(exception);
        LogFailure(request, exception, logLevel);

        var failureResult = ResultFailureFactory.Create<TResponse>(userMessage);
        return Handled(failureResult);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Classification
    // ─────────────────────────────────────────────────────────────────────────

    private static bool IsGatewayException(Exception ex) =>
        ex is SmsGatewayException
            or HttpRequestException
            or TaskCanceledException      // HttpClient timeout when caller token NOT cancelled
            or JsonException;

    // ─────────────────────────────────────────────────────────────────────────
    // Message mapping
    // ─────────────────────────────────────────────────────────────────────────

    private static (string Message, LogLevel Level) MapToUserMessage(Exception ex) => ex switch
    {
        // Custom wrapper — status code carries the semantics.
        SmsGatewayException sms => (MapStatusCode(sms.StatusCode, sms.Message), MapLogLevel(sms.StatusCode)),

        // Raw transport failures (mostly come from a misbehaving client wrapper).
        HttpRequestException http => (
            "Could not reach the SMS gateway. Verify the device is powered on and connected to the network.",
            LogLevel.Warning),

        // HttpClient timeout (TaskCanceledException with inner TimeoutException, or
        // TaskCanceledException thrown outside cancellation). Caller cancellation
        // was filtered out above.
        TaskCanceledException => (
            "The SMS gateway did not respond in time. Please try again shortly.",
            LogLevel.Warning),

        // Gateway returned non-JSON / truncated body.
        JsonException => (
            "The SMS gateway returned an invalid response. Please retry; if the problem persists, verify the gateway firmware.",
            LogLevel.Error),

        _ => ("An unexpected error occurred while contacting the SMS gateway.", LogLevel.Error)
    };

    private static string MapStatusCode(int? statusCode, string fallback) => statusCode switch
    {
        400 => "The SMS gateway rejected the request as malformed.",
        401 or 403 => "The SMS gateway rejected our credentials. Check the API key in SMS Gateway settings.",
        404 => "The SMS gateway endpoint was not found. Verify the Base URL in SMS Gateway settings.",
        408 => "The SMS gateway timed out processing the request.",
        429 => "The SMS gateway is rate-limiting requests. Please retry in a moment.",
        >= 500 => "The SMS gateway reported an internal error. Please try again later.",
        _ => fallback
    };

    private static LogLevel MapLogLevel(int? statusCode) => statusCode switch
    {
        401 or 403 or 404 => LogLevel.Error,   // config problem — operator action required
        429 or >= 500 => LogLevel.Warning,     // transient / retryable
        400 or 408 => LogLevel.Warning,
        _ => LogLevel.Warning
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Logging
    // ─────────────────────────────────────────────────────────────────────────

    private void LogFailure(TRequest request, Exception exception, LogLevel level)
    {
        var requestName = typeof(TRequest).Name;

        if (exception is SmsGatewayException sms)
        {
            _logger.Log(level, sms,
                "SMS gateway failure in {RequestType}. Operation={Operation} Status={Status}",
                requestName, sms.Operation, sms.StatusCode);
            return;
        }

        _logger.Log(level, exception,
            "SMS gateway failure in {RequestType} ({ExceptionType})",
            requestName, exception.GetType().Name);
    }
}