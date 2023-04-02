using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RSSCargo.DAL.Models;

namespace RSSCargo.DAL.DataContext;

public class RssCargoContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public RssCargoContext(DbContextOptions<RssCargoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; } = null!;

    public virtual DbSet<CargoFeed> CargoFeeds { get; set; } = null!;

    public virtual DbSet<UserCargo> UserCargos { get; set; } = null!;

    public virtual DbSet<UserFeed> UserFeeds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id).HasName("users_pk");
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UserName).HasColumnName("username");
            entity.Property(p => p.Email).HasColumnName("email");
            entity.Property(p => p.PasswordHash).HasColumnName("password_hash");
        });

        modelBuilder.Entity<IdentityRole<int>>().ToTable("roles");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cargos_pk");

            entity.ToTable("cargos");

            entity.HasIndex(e => e.Name, "cargos_name_index").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CargoFeed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cargo_feeds_pk");

            entity.ToTable("cargo_feeds");

            entity.HasIndex(e => new { e.CargoId, e.RssFeed }, "cargo_feeds_rss_feed_index").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CargoId).HasColumnName("cargo_id");
            entity.Property(e => e.RssFeed)
                .HasMaxLength(2048)
                .HasColumnName("rss_feed");

            entity.HasOne(d => d.Cargo).WithMany(p => p.CargoFeeds)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("cargo_feeds_cargo_id_fk");
        });

        modelBuilder.Entity<UserCargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_cargos_pk");

            entity.ToTable("user_cargos");

            entity.HasIndex(e => new { e.UserId, e.CargoId }, "user_cargos_cargo_id_index").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CargoId).HasColumnName("cargo_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Cargo).WithMany(p => p.UserCargos)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("user_cargos_cargo_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.UserCargos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_cargos_user_id_fk");
        });

        modelBuilder.Entity<UserFeed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_feeds_pk");

            entity.ToTable("user_feeds");

            entity.HasIndex(e => new { e.UserId, e.RssFeed }, "user_feeds_rss_feed_index").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.RssFeed)
                .HasMaxLength(2048)
                .HasColumnName("rss_feed");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserFeeds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_cargos_user_id_fk");
        });
    }
}