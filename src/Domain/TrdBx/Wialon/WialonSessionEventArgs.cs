namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Event arguments for session-related events
/// </summary>
public class WialonSessionEventArgs : EventArgs
{
    /// <summary>
    /// Current session ID (may be null if session is inactive)
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Indicates whether the session is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Human-readable message describing the event
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Timestamp when the event occurred (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}