using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gascar.Data;

namespace Gascar.Controllers;

[Authorize(Roles = "Automobilista")]
public class AutomobilistaController : Controller
{
    private readonly ApplicationDbContext _db;

    public AutomobilistaController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Dashboard()
    {
        return View();
    }
}
