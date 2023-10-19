using PSL.Contracts;

namespace PSL.Services;

public interface IEntryService
{
    public Task<Dictionary<EState, TimeSpan>> GetTimeSpentInStateForUserAndTimeSpanAsync(int userId, TimeSpan timeSpan);
}