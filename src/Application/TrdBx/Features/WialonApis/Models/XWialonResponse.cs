using System.Text.Json.Serialization;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Models;
public class XWialonResponse
{
    [JsonPropertyName("error")]
    public int ErrorCode { get; set; }
}
