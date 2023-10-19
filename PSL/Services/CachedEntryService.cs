using Microsoft.Extensions.Caching.Memory;
using PSL.Contracts;

namespace PSL.Services;

public class CachedEntryService: IEntryService
{
    private readonly IMemoryCache _cache;
    private readonly EntryService _entryServiceImplementation;

    public CachedEntryService(EntryService entryServiceImplementation, IMemoryCache cache)
    {
        _entryServiceImplementation = entryServiceImplementation;
        _cache = cache;
    }

    public async Task<Dictionary<EState, TimeSpan>> GetTimeSpentInStateForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var key = $"time_spent_in_state_{userId}_{timeSpan}";
        return await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _entryServiceImplementation.GetTimeSpentInStateForUserAndTimeSpanAsync(userId, timeSpan);
        }) ?? throw new Exception("Something went wrong with the cache");
    }
}