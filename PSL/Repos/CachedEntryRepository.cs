using Microsoft.Extensions.Caching.Memory;
using PSL.Data.Presence;

namespace PSL.Repos;

class CachedEntryRepository : IEntryRepository
{
    private readonly EntryRepository _entryRepository;
    private readonly IMemoryCache _cache;
    
    public CachedEntryRepository(EntryRepository entryRepository, IMemoryCache cache)
    {
        _entryRepository = entryRepository;
        _cache = cache;
    }

    public Task<Entry?> GetLatestEntryForUserAsync(int userId)
    {
        return _entryRepository.GetLatestEntryForUserAsync(userId);
    }

    public Task<Dictionary<int, Entry>> GetLatestEntriesForUsersAsync(IEnumerable<int> userIds)
    {
        var key = $"latest_entries_{string.Join("_", userIds)}";
        return _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
            return await _entryRepository.GetLatestEntriesForUsersAsync(userIds);
        });
    }

    public Task<Entry?> GetLastEntryForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var key = $"last_entry_{userId}_{timeSpan}";
        return _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            return await _entryRepository.GetLastEntryForUserAndTimeSpanAsync(userId, timeSpan);
        });
    }

    public async Task<IList<Entry>> GetEntriesForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var key = $"entries_{userId}_{timeSpan}";
        return await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            return await _entryRepository.GetEntriesForUserAndTimeSpanAsync(userId, timeSpan);
        }) ?? throw new Exception("Entries not found");
    }

    public async Task<IList<Entry>> GetEntriesForUserAsync(int userId)
    {
        var key = $"entries_{userId}_all";
        return await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            return await _entryRepository.GetEntriesForUserAsync(userId);
        }) ?? throw new Exception("Entries not found");
    }
}