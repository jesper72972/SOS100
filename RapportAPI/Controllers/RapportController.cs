using Microsoft.AspNetCore.Mvc;
using RapportAPI.Services;

namespace RapportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RapportController : ControllerBase
{
    private readonly RapportService _rapportService;

    public RapportController(RapportService rapportService)
    {
        _rapportService = rapportService;
    }

    [HttpGet("statistik")]
    public async Task<IActionResult> HamtaStatistik()
    {
        try
        {
            var resultat = await _rapportService.HamtaStatistikAsync();
            return Ok(resultat);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(503, new { fel = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { fel = "Ett oväntat fel inträffade.", detaljer = ex.Message });
        }
    }
}