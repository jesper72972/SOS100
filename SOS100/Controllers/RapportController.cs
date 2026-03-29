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
        try
        {
            var statistik = await _rapportService.GetStatistikAsync();
            if (statistik == null)
            {
                TempData["Felmeddelande"] = "Statistik kunde inte hämtas – API:et returnerade ett tomt svar.";
                return View(new SOS100.Models.RapportStatistik());
            }
            return View(statistik);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Felmeddelande"] = ex.Message;
            return View(new SOS100.Models.RapportStatistik());
        }
        catch (Exception ex)
        {
            TempData["Felmeddelande"] = $"Ett oväntat fel inträffade: {ex.Message}";
            return View(new SOS100.Models.RapportStatistik());
        }
    }
}