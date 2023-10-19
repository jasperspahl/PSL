using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSL.Contracts;
using PSL.Data.Presence;
using PSL.Repos;
using PSL.Services;

namespace PSL.Pages
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PSL.Data.Presence.PresenceStoreContext _context;
        private readonly IEntryRepository _entryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEntryService _entryService;

        public DetailsModel(PSL.Data.Presence.PresenceStoreContext context, IEntryRepository entryRepository, IUserRepository userRepository, IEntryService entryService)
        {
            _context = context;
            _entryRepository = entryRepository;
            _userRepository = userRepository;
            _entryService = entryService;
        }

        public new Data.Presence.User User { get; set; } = default!; 
        public new Entry Entry { get; set; } = default!;
        public new IList<Entry> Entries { get; set; } = default!;
        
        public new Dictionary<EState, TimeSpan> OnlineTimes { get; set; } = default!;
        
        public new DateTime Date { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(int? id, TimeSpan offset)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            Entry = (await _entryRepository.GetLatestEntryForUserAsync((int) id))!;
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            Date = DateTime.UtcNow - offset;
            var entries = await _entryRepository.GetEntriesForUserAndTimeSpanAsync((int) id, offset);
            if (entries.Count == 0)
            {
                return NotFound();
            }
            User = user;

            Entries = entries;

            OnlineTimes = await _entryService.GetTimeSpentInStateForUserAndTimeSpanAsync((int) id, offset);
            
            return Page();
        }
    }
}
