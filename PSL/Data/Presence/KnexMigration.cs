namespace PSL.Data.Presence;

public partial class KnexMigration
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Batch { get; set; }

    public DateTime? MigrationTime { get; set; }
}
