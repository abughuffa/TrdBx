using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Models;
public class Unit
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("nm")]
    public string Name { get; set; } = string.Empty;
}
