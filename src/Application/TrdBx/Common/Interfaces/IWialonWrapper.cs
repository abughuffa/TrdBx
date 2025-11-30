using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IWialonWrapper
{
    Task<bool> TryDisconnect();
    Task<bool> TryConnect();
    Task<bool> CleanUpResults();
    void UpdateToken(string token);



}