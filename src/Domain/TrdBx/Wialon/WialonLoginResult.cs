namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Result of a login attempt to Wialon
/// </summary>
public class WialonLoginResult
{
    /// <summary>
    /// Indicates if login was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The session ID if login was successful
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Host information from the login response
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// Error message if login failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// User information from the login response
    /// </summary>
    public WialonUserInfo? User { get; set; }
}