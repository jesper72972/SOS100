using Microsoft.EntityFrameworkCore;
using formaner_API.Models;

namespace formaner_API.Data;

public class DbService : DbContext
{
    public DbService(DbContextOptions<DbService> options) 
        : base(options) {}
    
   public  DbSet<Formaner> Formaners { get; set; }
}