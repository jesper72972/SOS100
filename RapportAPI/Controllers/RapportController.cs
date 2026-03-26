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
    public IActionResult HamtaStatistik()
    {
        var resultat = _rapportService.HamtaStatistik();
        return Ok(resultat);
    }
}