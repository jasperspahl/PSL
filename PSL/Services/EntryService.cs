using PSL.Contracts;
using PSL.Repos;

namespace PSL.Services;

public class EntryService : IEntryService
{
    private readonly IEntryRepository _entryRepository;

    public EntryService(IEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }

    public async Task<Dictionary<EState, TimeSpan>> GetTimeSpentInStateForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan)
    {
        var onlineTimes = new Dictionary<EState, TimeSpan>();
        int i;
        for (i = 1; i < 5; i++)
            onlineTimes.Add((EState)i, TimeSpan.Zero);
        
        var initialEntry = await _entryRepository.GetLastEntryForUserAndTimeSpanAsync(userId, timeSpan);
        var entries = await _entryRepository.GetEntriesForUserAndTimeSpanAsync(userId, timeSpan);
        EState lastState;
        var lastTime= DateTime.UtcNow - timeSpan;
        i = entries.Count - 1;
        if (initialEntry == null)
        {
            lastState = (EState)entries[^1].StateId;
            lastTime = entries[^1].CreatedAt;
            i--;
        }
        else
        {
            lastState = (EState)initialEntry.StateId;
        }
        
        while ( i >= 0)
        {
            if (entries[i].StateId == (int)lastState)
            {
                i--;
                continue;
            }
            
            var time = entries[i].CreatedAt - lastTime;
            onlineTimes[lastState] += time;
            lastState = (EState)entries[i].StateId;
            lastTime = entries[i].CreatedAt;
            i--;
        }
        onlineTimes[lastState] += DateTime.UtcNow - lastTime;
        return onlineTimes;
    }
}