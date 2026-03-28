using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapportAPI.Data;
using RapportAPI.Models;

namespace RapportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RapportPostController : ControllerBase
{
    private readonly DatabasContext _context;

    public RapportPostController(DatabasContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RapportPost>>> GetAll()
    {
        var poster = await _context.RapportPoster
            .OrderByDescending(r => r.Skapad)
            .ToListAsync();

        return Ok(poster);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RapportPost>> GetById(int id)
    {
        var rapportPost = await _context.RapportPoster.FindAsync(id);

        if (rapportPost == null)
        {
            return NotFound();
        }

        return Ok(rapportPost);
    }

    [HttpPost]
    public async Task<ActionResult<RapportPost>> Create(RapportPost rapportPost)
    {
        _context.RapportPoster.Add(rapportPost);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = rapportPost.Id }, rapportPost);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RapportPost uppdateradRapportPost)
    {
        if (id != uppdateradRapportPost.Id)
        {
            return BadRequest("Id i URL matchar inte objektets Id.");
        }

        var finns = await _context.RapportPoster.AnyAsync(r => r.Id == id);
        if (!finns)
        {
            return NotFound();
        }

        _context.Entry(uppdateradRapportPost).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rapportPost = await _context.RapportPoster.FindAsync(id);

        if (rapportPost == null)
        {
            return NotFound();
        }

        _context.RapportPoster.Remove(rapportPost);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}