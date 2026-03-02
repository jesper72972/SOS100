using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;

namespace SOS100.Controllers;

public class HRController : Controller
{
    public IActionResult HR()
    {
        return View();
    }
}
/*
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Formaner newFormaner)
    {
        using (var dbContext = new formanerDbContext())
        {
            dbContext.Formaners.Add(newFormaner);
            dbContext.SaveChanges();
        }
        return RedirectToAction("HR");
    }

}*/