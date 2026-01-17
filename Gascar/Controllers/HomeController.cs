using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gascar.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Dashboard()
    {
        if (User.IsInRole("Admin"))
            return View("AdminDashboard");

        return View("DriverDashboard");
    }
}
