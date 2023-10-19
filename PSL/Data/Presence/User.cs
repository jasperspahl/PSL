namespace PSL.Data.Presence;

public partial class User
{
    public int Id { get; set; }

    public string DiscordId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
