using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using SOS100.Services;
using SOS100.Models;

namespace SOS100.Controllers;

public class FormanController : Controller
{
    private FormanService _formanService;

    public FormanController(FormanService formanService)
    {
        _formanService = formanService;
    }
    public async Task<IActionResult> Forman()
    {
       Formaner[] formans = await _formanService.GetFormans();
        return View(formans); 
    }
    
   
   
}