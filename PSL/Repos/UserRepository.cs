using Microsoft.EntityFrameworkCore;
using PSL.Data.Presence;

namespace PSL.Repos;

public class UserRepository : IUserRepository
{
    private readonly PresenceStoreContext _context;
    
    public UserRepository(PresenceStoreContext context)
    {
        _context = context;
    }
    public async Task<User> GetUserAsync(int id)
    {
        return await _context.Users.FirstAsync(x => x.Id == id);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _context.Users.ToListAsync();
    }
}