namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface ISmsCursorStore
{
    Task<long> GetAsync(string key, CancellationToken ct = default);
    Task SetAsync(string key, long value, CancellationToken ct = default);
}