namespace PSL.Data.Presence;

public partial class Entry
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int StateId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual State State { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
