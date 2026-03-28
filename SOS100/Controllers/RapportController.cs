using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100.Services;

namespace SOS100.Controllers;

[Authorize]
public class RapportController : Controller
{
    private readonly RapportService _rapportService;

    public RapportController(RapportService rapportService)
    {
        _rapportService = rapportService;
    }

    public async Task<IActionResult> Rapport()
    {
        var statistik = await _rapportService.GetStatistikAsync();
        return View(statistik);
    }
}