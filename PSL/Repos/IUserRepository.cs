using PSL.Data.Presence;

namespace PSL.Repos;

public interface IUserRepository
{
    public Task<User> GetUserAsync(int id);
    public Task<List<User>> GetUsersAsync();
}