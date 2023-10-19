namespace PSL.Data.Presence;

public partial class State
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
