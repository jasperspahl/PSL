using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSL.Contracts;
using PSL.Repos;

namespace PSL.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IEntryRepository _entryRepository;

        public IndexModel(IUserRepository userRepository, IEntryRepository entryRepository)
        {
            _userRepository = userRepository;
            _entryRepository = entryRepository;
        }

        public new IList<Data.Presence.User> User { get;set; } = default!;
        public new IDictionary<int, Data.Presence.Entry> Entries { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            var entries = await _entryRepository.GetLatestEntriesForUsersAsync(users.Select(x=>x.Id));
            users = users.OrderByDescending(x => entries[x.Id].CreatedAt).ToList();
            var onlineUsers = users.Where(x => entries[x.Id].StateId == (int)EState.Online).ToList();
            var otherUsers = users.Where(x => entries[x.Id].StateId != (int)EState.Online).ToList();
            User = onlineUsers.Concat(otherUsers).ToList();
            Entries = entries;
        }
    }
}
