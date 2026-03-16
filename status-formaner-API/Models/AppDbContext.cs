using Microsoft.EntityFrameworkCore;
using status_formaner_API.Models;

namespace status_formaner_API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ServiceStatus> ServiceStatuses { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceStatus>(entity =>
        {
            entity.ToTable("ServiceStatus");
            entity.HasKey(e => new { e.ID, e.ServicID });
            entity.HasIndex(e => e.ID).IsUnique();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
        });
    }
}
