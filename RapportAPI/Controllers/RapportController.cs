using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RapportController : ControllerBase
{
    private readonly AppDbContext _context;

    public RapportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rapport>>> Get()
    {
        return await _context.Rapporter.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Rapport>> Post(Rapport rapport)
    {
        _context.Rapporter.Add(rapport);
        await _context.SaveChangesAsync();
        return rapport;
    }
}