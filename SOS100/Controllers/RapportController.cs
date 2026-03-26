using Microsoft.AspNetCore.Mvc;
using SOS100.Services;

namespace SOS100.Controllers;

public class RapportController : Controller
{
    private readonly RapportService _rapportService;

    public RapportController(RapportService rapportService)
    {
        _rapportService = rapportService;
    }

    public async Task<IActionResult> Rapport()
    {
        var rapportStatistik = await _rapportService.HamtaRapportStatistik();
        return View(rapportStatistik);
    }
}