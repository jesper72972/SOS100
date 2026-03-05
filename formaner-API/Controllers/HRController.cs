using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using formaner_API.Models;
using formaner_API.Data;

namespace formaner_API.Controllers;

public class HRController : Controller
{
    public IActionResult HR()
    {
        return View();
    }
}