using GodkannadeAPI.Data;
using GodkannadeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GodkannadeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalsController : ControllerBase
{
    private readonly ApprovalDbContext _context;

    public ApprovalsController(ApprovalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Approval>>> GetApprovals()
    {
        return await _context.Approvals.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Approval>> GetApproval(int id)
    {
        var approval = await _context.Approvals.FindAsync(id);

        if (approval == null)
        {
            return NotFound();
        }

        return approval;
    }

    [HttpPost]
    public async Task<ActionResult<Approval>> CreateApproval(Approval approval)
    {
        _context.Approvals.Add(approval);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetApproval), new { id = approval.Id }, approval);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApproval(int id, Approval approval)
    {
        if (id != approval.Id)
        {
            return BadRequest();
        }

        _context.Entry(approval).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApproval(int id)
    {
        var approval = await _context.Approvals.FindAsync(id);

        if (approval == null)
        {
            return NotFound();
        }

        _context.Approvals.Remove(approval);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}