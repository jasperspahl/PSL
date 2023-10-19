using Microsoft.Extensions.Caching.Memory;
using PSL.Data.Presence;

namespace PSL.Repos;

public class CachedUserRepository: IUserRepository
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;
    
    public CachedUserRepository(UserRepository userRepository, IMemoryCache cache)
    {
        _userRepository = userRepository;
        _cache = cache;
    }
    public async Task<User> GetUserAsync(int id)
    {
        var key = $"user_{id}";
        return await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _userRepository.GetUserAsync(id);
        }) ?? throw new Exception("User not found");
    }

    public async Task<List<User>> GetUsersAsync()
    {
        const string key = "users_all";
        return await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _userRepository.GetUsersAsync();
        }) ?? throw new Exception("some thing went wrong with the cache");
    }
}