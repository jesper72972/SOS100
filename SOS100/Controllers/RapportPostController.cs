using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;
using SOS100.Services;

namespace SOS100.Controllers;

[Authorize]
public class RapportPostController : Controller
{
    private readonly RapportPostService _service;

    public RapportPostController(RapportPostService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var poster = await _service.GetAllAsync();
        return View(poster);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RapportPostDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _service.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var post = await _service.GetByIdAsync(id);
        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RapportPostDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _service.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var post = await _service.GetByIdAsync(id);
        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}