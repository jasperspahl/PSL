using Microsoft.EntityFrameworkCore;
using PSL.Contracts;
using PSL.Data.Presence;

namespace PSL.Repos;

class EntryRepository : IEntryRepository
{
    private readonly PresenceStoreContext _context;
    
    public EntryRepository(PresenceStoreContext context)
    {
        _context = context;
    }
    
    private static List<Entry> FilterEntries(IEnumerable<Entry> entries)
    {
        return entries.Aggregate(new List<Entry>(), (list, entry) =>
        {
            if (list.Count == 0)
            {
                list.Add(entry);
            }
            else
            {
                if (entry.StateId != list[^1].StateId)
                {
                    list.Add(entry);
                }
            }
            return list;
        });
    }

    public Task<Entry?> GetLatestEntryForUserAsync(int userId)
    {
        return _context.Entries.Where(x => x.UserId == userId)
            .Include(x => x.State)
            .OrderByDescending(e => e.CreatedAt).FirstOrDefaultAsync();
    }

    public Task<Dictionary<int, Entry>> GetLatestEntriesForUsersAsync(IEnumerable<int> userIds)
    {
        return _context.Entries.Where(x => userIds.Contains(x.UserId))
            .Include(x => x.State)
            .GroupBy(x => x.UserId)
            .Select(x => x.OrderByDescending(e => e.CreatedAt).FirstOrDefault())
            .ToDictionaryAsync(x => x.UserId, x => x);
    }

    public async Task<Entry?> GetLastEntryForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var date = DateTime.UtcNow - timeSpan;
        return await _context.Entries.Where(x => x.CreatedAt >= date && x.UserId == userId)
            .Include(x => x.State)
            .OrderByDescending(e => e.CreatedAt).FirstOrDefaultAsync();
    }

    public async Task<IList<Entry>> GetEntriesForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var date = DateTime.UtcNow - timeSpan;
        var entries = await _context.Entries.Where(x => x.CreatedAt >= date && x.UserId == userId)
            .Include(x => x.State)
            .OrderByDescending(e => e.CreatedAt).ToListAsync();
        return FilterEntries(entries);
    }

    public async Task<IList<Entry>> GetEntriesForUserAsync(int userId)
    {
        var entries = await _context.Entries.Where(x => x.UserId == userId)
            .Include(x => x.State)
            .OrderByDescending(e => e.CreatedAt).ToListAsync();
        return FilterEntries(entries);
    }
}