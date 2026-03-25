using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Rapport> Rapporter { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}