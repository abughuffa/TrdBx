using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// User information returned from Wialon API
/// </summary>
public class WialonUserInfo
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// User's display name
    /// </summary>
    [JsonPropertyName("nm")]
    public string? Name { get; set; }

    /// <summary>
    /// Creation timestamp (Unix timestamp)
    /// </summary>
    [JsonPropertyName("crt")]
    public long Created { get; set; }

    /// <summary>
    /// User properties (custom fields)
    /// </summary>
    [JsonPropertyName("prp")]
    public Dictionary<string, string>? Properties { get; set; }
}