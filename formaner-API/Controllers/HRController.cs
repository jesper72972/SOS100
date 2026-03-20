using Microsoft.AspNetCore.Mvc;

namespace formaner_API.Controllers;

public class HRController : Controller
{
    public IActionResult HR()
    {
        return View();
    }
}