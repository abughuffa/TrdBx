using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Response model for Wialon token login API
/// </summary>
public class WialonLoginResponse : WialonResponse
{
    /// <summary>
    /// Session ID (eid) returned after successful login
    /// </summary>
    [JsonPropertyName("eid")]
    public string? SessionId { get; set; }

    /// <summary>
    /// Host server information
    /// </summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>
    /// Authentication hash for additional operations
    /// </summary>
    [JsonPropertyName("au")]
    public string? AuthHash { get; set; }

    /// <summary>
    /// Timestamp of the login response
    /// </summary>
    [JsonPropertyName("tm")]
    public long Timestamp { get; set; }

    /// <summary>
    /// User information of the logged-in user
    /// </summary>
    [JsonPropertyName("user")]
    public WialonUserInfo? User { get; set; }

    /// <summary>
    /// Override to also check that SessionId is not null or empty
    /// </summary>
    public new bool IsSuccess => ErrorCode == 0 && !string.IsNullOrEmpty(SessionId);
}