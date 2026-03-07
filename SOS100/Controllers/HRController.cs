using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;
using SOS100.Services;

namespace SOS100.Controllers;

[Authorize]
public class HRController : Controller
{
    public IActionResult HR()
    {
        return View();
    }

     private readonly FormanService _formanService;
    
        public HRController(FormanService formanService) 
        {
            _formanService = formanService;
        }
        
    [HttpPost]
    public async Task<IActionResult> Create(Formaner formaner)
    {
        await _formanService.CreateForman(formaner);
        return RedirectToAction("HR");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Formaner formaner)
    {
        await _formanService.EditForman(formaner);
        return RedirectToAction("HR");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Formaner formaner)
    {        
        await _formanService.DeleteForman(formaner.ID);
        return RedirectToAction("HR");
    }
   
   
   


}