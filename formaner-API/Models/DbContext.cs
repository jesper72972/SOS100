using SOS100.Models;
using Microsoft.EntityFrameworkCore;

namespace SOS100.Models;


    public class formanerDbContext : DbContext
    {
        public DbSet<Formaner> Formaners { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource = FormanerDB.db");
        }
    }
