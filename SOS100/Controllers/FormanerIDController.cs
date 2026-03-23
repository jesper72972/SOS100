using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using SOS100.Services;
using SOS100.Models;
namespace SOS100.Controllers;
[Authorize]
public class FormanerIDController : Controller
{
    private FormanService _formanService;

    public FormanerIDController(FormanService formanService)
    {
        _formanService = formanService;
    } 
    public async Task<IActionResult> FormanerID()
    {
        Formaner[] formans = await _formanService.GetFormans();
        return View(formans); 
    }
   
}