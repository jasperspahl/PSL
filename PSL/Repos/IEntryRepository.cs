using PSL.Contracts;
using PSL.Data.Presence;

namespace PSL.Repos;

public interface IEntryRepository
{
    public Task<Entry?> GetLatestEntryForUserAsync(int userId);
    public Task<Dictionary<int, Entry>> GetLatestEntriesForUsersAsync(IEnumerable<int> userIds);
    public Task<Entry?> GetLastEntryForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan);
    public Task<IList<Entry>> GetEntriesForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan);
    public Task<IList<Entry>> GetEntriesForUserAsync(int userId);
}