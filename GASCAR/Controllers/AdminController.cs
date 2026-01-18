using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Dashboard()
    {
        var posti = _db.PostiAuto.ToList();
        return View(posti);
    }

    [HttpPost]
    public async Task<IActionResult> AggiornaCosti(decimal costoOra, decimal costoKw)
    {
        var config = await _db.Configurazioni.FirstAsync();
        config.CostoOraSosta = costoOra;
        config.CostoKW = costoKw;
        await _db.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    public IActionResult Pagamenti()
    {
        return View(_db.Pagamenti.ToList());
    }
}
