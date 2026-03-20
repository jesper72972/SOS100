using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using status_formaner_API.Data;
using status_formaner_API.Models;

namespace status_formaner_API.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CommentsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<Comment[]> GetAll() =>
        await _db.Comments.ToArrayAsync();

    [HttpGet("{statusId}")]
    public async Task<Comment[]> GetByStatus(int statusId) =>
        await _db.Comments.Where(c => c.StatusOBJ == statusId).ToArrayAsync();

    [HttpPost]
    public async Task<IActionResult> Create(Comment comment)
    {
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return Ok(comment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _db.Comments.FindAsync(id);
        if (comment == null) return NotFound();
        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
        return Ok(comment);
    }

    [HttpDelete("byStatus/{statusId}")]
    public async Task<IActionResult> DeleteByStatus(int statusId)
    {
        var comments = await _db.Comments.Where(c => c.StatusOBJ == statusId).ToListAsync();
        _db.Comments.RemoveRange(comments);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
