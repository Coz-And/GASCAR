using Gascar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gascar.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login() => View();

    [HttpPost]
    [HttpPost]
public async Task<IActionResult> Login(string email, string password)
{
    var result = await _signInManager.PasswordSignInAsync(
        email, password, false, false);

    if (!result.Succeeded)
    {
        ViewBag.Error = "Invalid login";
        return View();
    }

    var user = await _userManager.FindByEmailAsync(email);

    if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
        return RedirectToAction("Dashboard", "Admin");

    return RedirectToAction("Dashboard", "Automobilista");
}


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
