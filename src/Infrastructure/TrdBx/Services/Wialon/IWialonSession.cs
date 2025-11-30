using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Blazor.Infrastructure.Services.Wialon;
public interface IWialonSession
{
    void UpdateToken(string newToken);
    Task<bool> TryDisconnect();
    Task<bool> TryConnect();
    Task<JObject> SendRequest(string path, string param = null);
    Task<JObject> SendRequest(string path, params KeyValuePair<string, string>[] parameters);
    ValueTask DisposeAsync();
}
