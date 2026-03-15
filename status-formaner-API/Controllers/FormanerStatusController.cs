using Microsoft.AspNetCore.Mvc;
using status_formaner_API.Models;

namespace status_formaner_API.Controllers;

[ApiController]
[Route("[controller]")]
public class FormanerStatusController : ControllerBase
{
    private static readonly List<FormanerStatus> _statuses = new();
    private static int _nextId = 1;

    [HttpGet]
    public FormanerStatus[] GetAll() => _statuses.ToArray();

    [HttpPost]
    public IActionResult Create(FormanerStatus newStatus)
    {
        newStatus.ID = _nextId++;
        _statuses.Add(newStatus);
        return Ok(newStatus);
    }

    [HttpPost("Edit")]
    public IActionResult Edit(FormanerStatus updatedStatus)
    {
        var existing = _statuses.FirstOrDefault(s => s.ID == updatedStatus.ID);
        if (existing == null) return NotFound();

        existing.EmployeeName = updatedStatus.EmployeeName;
        existing.FormanerTitle = updatedStatus.FormanerTitle;
        existing.Status = updatedStatus.Status;
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _statuses.FirstOrDefault(s => s.ID == id);
        if (existing == null) return NotFound();

        _statuses.Remove(existing);
        return Ok(existing);
    }
}
