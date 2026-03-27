
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;

namespace SOS100.Controllers;

public class AccountController : Controller
{
    public IActionResult Index(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Account account, string returnUrl)
    {
        bool accountValid = account.Username == "admin" && account.Password == "abc123";

        if (!accountValid)
        {
            ViewBag.ErrorMessage = "Login failed: Wrong username or password";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, account.Username));
        var props = new AuthenticationProperties { IsPersistent = true };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);

        if (String.IsNullOrEmpty(returnUrl))
        {
            return RedirectToAction("Index", "Home");
        }
        
        return Redirect(returnUrl);

    }

    public async Task<IActionResult> SignOutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home" );
    }
}
