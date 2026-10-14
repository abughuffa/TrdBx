
namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class EfSmsCursorStore : ISmsCursorStore
{

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly ILogger<EfSmsCursorStore> _log;
    
   public EfSmsCursorStore(
       IApplicationDbContextFactory dbContextFactory, ILogger<EfSmsCursorStore> log)
    {
     
        _log = log;
   
       _dbContextFactory = dbContextFactory;
    }

   public async Task<long> GetAsync(string key, CancellationToken ct = default)
    {
       await using var _db = await _dbContextFactory.CreateAsync();

        try
        {
            var row = await _db.SmsCursors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == key, ct);
            return row?.Value ?? 0L;
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "SMS cursor read failed for '{Key}'; defaulting to 0.", key);
            return 0L;
        }
    }

    public async Task SetAsync(string key, long value, CancellationToken ct = default)
    {
       await using var _db = await _dbContextFactory.CreateAsync();
                var row = await _db.SmsCursors.FirstOrDefaultAsync(x => x.Key == key, ct);
        if (row is null)
            _db.SmsCursors.Add(new SmsCursor { Key = key, Value = value });
        else
            row.Value = value;

        await _db.SaveChangesAsync(ct);
    }
}