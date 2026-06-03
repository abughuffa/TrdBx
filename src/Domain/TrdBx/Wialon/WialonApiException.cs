namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Exception thrown when Wialon API returns an error
/// </summary>
public class WialonApiException : Exception
{
    /// <summary>
    /// Wialon API error code
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Creates a new instance of WialonApiException
    /// </summary>
    /// <param name="errorCode">Wialon API error code</param>
    /// <param name="message">Error message</param>
    public WialonApiException(int errorCode, string message) 
        : base($"Wialon API Error [{errorCode}]: {message}")
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Creates a new instance of WialonApiException with inner exception
    /// </summary>
    /// <param name="errorCode">Wialon API error code</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public WialonApiException(int errorCode, string message, Exception innerException) 
        : base($"Wialon API Error [{errorCode}]: {message}", innerException)
    {
        ErrorCode = errorCode;
    }
}