using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;

namespace SOS100.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}