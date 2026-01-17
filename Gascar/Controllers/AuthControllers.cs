using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Gascar.Models;

namespace Gascar.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(
            username, password, false, false);

        if (result.Succeeded)
            return RedirectToAction("Dashboard", "Home");

        return View();
    }
}
