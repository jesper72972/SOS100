using Microsoft.EntityFrameworkCore;
namespace formaner_API.Models;

    public class FormanerDbContext : DbContext
    {
        public DbSet<Formaner> Formaners { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource = demo-service-dev.db");
        }
    }
