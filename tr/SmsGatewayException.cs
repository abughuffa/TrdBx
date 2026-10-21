// CleanArchitecture.Blazor.Application/Common/ExceptionHandlers/SmsGatewayException.cs
namespace CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

/// <summary>
/// Raised when the SMS gateway is unreachable, rejects a request, times out,
/// or returns a response that cannot be interpreted. Carries optional
/// diagnostics (operation name, HTTP status) that would otherwise be lost
/// when the exception is converted to a <c>Result.Failure</c>.
/// </summary>
public sealed class SmsGatewayException : Exception
{
    /// <summary>Logical operation, e.g. "SendSms", "GetInbox".</summary>
    public string? Operation { get; }

    /// <summary>HTTP status code returned by the gateway, when applicable.</summary>
    public int? StatusCode { get; }

    public SmsGatewayException(
        string message,
        string? operation = null,
        int? statusCode = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Operation = operation;
        StatusCode = statusCode;
    }
}