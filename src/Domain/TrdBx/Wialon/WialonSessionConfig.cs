namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Configuration settings for Wialon session management
/// </summary>
public class WialonSessionConfig
{
    /// <summary>
    /// Base URL of the Wialon server (e.g., https://cms.eagleeye.ly)
    /// </summary>
    public string BaseUrl { get; set; } = "https://cms.eagleeye.ly";

    /// <summary>
    /// API token for authentication
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Interval in minutes between keep-alive requests to prevent session timeout
    /// </summary>
    public int KeepAliveIntervalMinutes { get; set; } = 3;

    /// <summary>
    /// Maximum session idle timeout in minutes before session is considered expired
    /// </summary>
    public int SessionTimeoutMinutes { get; set; } = 5;

    /// <summary>
    /// Maximum number of retry attempts for failed API calls
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Base delay in milliseconds between retry attempts (exponential backoff applied)
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Whether to automatically reconnect when session expires
    /// </summary>
    public bool AutoReconnect { get; set; } = true;

    /// <summary>
    /// Interval in seconds between health check operations
    /// </summary>
    public int HealthCheckIntervalSeconds { get; set; } = 30;

    /// <summary>
    /// Buffer time in minutes before token expiry to trigger warning
    /// </summary>
    public int TokenExpiryBufferMinutes { get; set; } = 5;
}