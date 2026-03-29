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
        try
        {
            var poster = await _service.GetAllAsync();
            return View(poster);
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte hämta rapportposter: {ex.Message}";
            return View(new List<RapportPostDto>());
        }
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

        try
        {
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte skapa rapportpost: {ex.Message}";
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var post = await _service.GetByIdAsync(id);
            return View(post);
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte hämta rapportpost: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RapportPostDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte uppdatera rapportpost: {ex.Message}";
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var post = await _service.GetByIdAsync(id);
            return View(post);
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte hämta rapportpost: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Kunde inte ta bort rapportpost: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}