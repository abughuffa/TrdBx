namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Health status information for Wialon service
/// </summary>
public class WialonHealthStatus
{
    /// <summary>
    /// Overall health status (session active and valid)
    /// </summary>
    public bool IsHealthy { get; set; }

    /// <summary>
    /// Whether the session is currently active
    /// </summary>
    public bool IsSessionActive { get; set; }

    /// <summary>
    /// Current session ID (if active)
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Last activity timestamp (UTC)
    /// </summary>
    public DateTime LastActivity { get; set; }

    /// <summary>
    /// Last health check timestamp (UTC)
    /// </summary>
    public DateTime LastHealthCheck { get; set; }

    /// <summary>
    /// Last error message (if any)
    /// </summary>
    public string? LastErrorMessage { get; set; }

    /// <summary>
    /// List of recent API calls (last 100)
    /// </summary>
    public List<WialonApiCall> RecentApiCalls { get; set; } = new();

    /// <summary>
    /// Total number of API calls made
    /// </summary>
    public int TotalApiCalls { get; set; }

    /// <summary>
    /// Number of failed API calls
    /// </summary>
    public int FailedApiCalls { get; set; }

    /// <summary>
    /// Success rate percentage of API calls
    /// </summary>
    public double SuccessRate { get; set; }
}

/// <summary>
/// Record of a single API call to Wialon
/// </summary>
public class WialonApiCall
{
    /// <summary>
    /// Name of the service/API method called
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the call was made (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Whether the call was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error code if the call failed
    /// </summary>
    public int? ErrorCode { get; set; }

    /// <summary>
    /// Duration of the API call in milliseconds
    /// </summary>
    public long DurationMs { get; set; }
}