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
        var resultat = await _rapportService.HamtaStatistikAsync();
        return Ok(resultat);
    }
}