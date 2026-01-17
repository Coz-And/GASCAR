using Gascar.Data;
using Gascar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Login() => View();

    // POST
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return View("Login");

        var result = _hasher.VerifyHashedPassword(user, user.Password, password);
        if (result == PasswordVerificationResult.Failed)
            return View("Login");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("IsPremium", user.IsPremium.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}
