using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Models;
public class UnitListResponse : XWialonResponse
{
    [JsonPropertyName("items")]
    public List<Unit> Units { get; set; } = new();
}
