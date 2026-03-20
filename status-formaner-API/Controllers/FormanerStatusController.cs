using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using status_formaner_API.Data;
using status_formaner_API.Models;

namespace status_formaner_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceStatusController : ControllerBase
{
    private readonly AppDbContext _db;

    public ServiceStatusController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ServiceStatus[]> GetAll() =>
        await _db.ServiceStatuses.ToArrayAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var status = await _db.ServiceStatuses.FirstOrDefaultAsync(s => s.ID == id);
        return status == null ? NotFound() : Ok(status);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        var status = await _db.ServiceStatuses.FirstOrDefaultAsync(s => s.ID == id);
        if (status == null) return NotFound();
        status.Status = newStatus;
        await _db.SaveChangesAsync();
        return Ok(status);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await _db.ServiceStatuses.FirstOrDefaultAsync(s => s.ID == id);
        if (status == null) return NotFound();
        _db.ServiceStatuses.Remove(status);
        await _db.SaveChangesAsync();
        return Ok(status);
    }
}

