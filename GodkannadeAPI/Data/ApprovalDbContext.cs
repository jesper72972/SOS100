using GodkannadeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GodkannadeAPI.Data;

public class ApprovalDbContext : DbContext
{
    public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Approval> Approvals { get; set; }
}