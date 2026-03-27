using Microsoft.EntityFrameworkCore;
using RapportAPI.Models;

namespace RapportAPI.Data;

public class DatabasContext : DbContext
{
    public DatabasContext(DbContextOptions<DatabasContext> options)
        : base(options)
    {
    }

    public DbSet<Forman> Formaner { get; set; }
    public DbSet<Ansokan> Ansokningar { get; set; }
}