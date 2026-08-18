using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Domain;

/// <summary>
/// Base response model for all Wialon API responses
/// </summary>
public class WialonResponse
{
    /// <summary>
    /// Error code from Wialon API (0 = success)
    /// </summary>
    [JsonPropertyName("error")]
    public int ErrorCode { get; set; }

    /// <summary>
    /// Error reason message if error code is not 0
    /// </summary>
    [JsonPropertyName("reason")]
    public string? ErrorReason { get; set; }

    /// <summary>
    /// Indicates if the API call was successful
    /// </summary>
    public bool IsSuccess => ErrorCode == 0;

    /// <summary>
    /// Ensures the response was successful, otherwise throws an exception
    /// </summary>
    /// <exception cref="WialonApiException">Thrown when ErrorCode is not 0</exception>
    public void EnsureSuccess()
    {
        if (!IsSuccess)
        {
            throw new WialonApiException(ErrorCode, ErrorReason ?? $"Wialon API error code: {ErrorCode}");
        }
    }
}

/// <summary>
/// Generic response model for Wialon API responses with typed data
/// </summary>
/// <typeparam name="T">Type of the data payload</typeparam>
public class WialonResponse<T> : WialonResponse
{
    /// <summary>
    /// Typed data payload from the API response
    /// </summary>
    [JsonIgnore]
    public T? Data { get; set; }
}