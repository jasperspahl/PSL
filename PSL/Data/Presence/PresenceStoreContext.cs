using Microsoft.EntityFrameworkCore;

namespace PSL.Data.Presence;

public partial class PresenceStoreContext : DbContext
{
    public PresenceStoreContext()
    {
    }

    public PresenceStoreContext(DbContextOptions<PresenceStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entry> Entries { get; set; }

    public virtual DbSet<KnexMigration> KnexMigrations { get; set; }

    public virtual DbSet<KnexMigrationsLock> KnexMigrationsLocks { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Presence");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entry_pkey");

            entity.ToTable("entry");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.State).WithMany(p => p.Entries)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("entry_state_id_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.Entries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("entry_user_id_foreign");
        });

        modelBuilder.Entity<KnexMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("knex_migrations_pkey");

            entity.ToTable("knex_migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.MigrationTime).HasColumnName("migration_time");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<KnexMigrationsLock>(entity =>
        {
            entity.HasKey(e => e.Index).HasName("knex_migrations_lock_pkey");

            entity.ToTable("knex_migrations_lock");

            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.IsLocked).HasColumnName("is_locked");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("state_pkey");

            entity.ToTable("state");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.DiscordId, "user_discord_id_index");

            entity.HasIndex(e => e.DiscordId, "user_discord_id_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscordId).HasColumnName("discord_id");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
