using Gascar.Data;
using Gascar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gascar.Controllers;

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
        var spots = _db.ParkingSpots.ToList();
        return View(spots);
    }

    public IActionResult Payments(DateTime? fromDate, DateTime? toDate, PaymentType? type)
    {
        var query = _db.Payments.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(p => p.Date >= fromDate);

        if (toDate.HasValue)
            query = query.Where(p => p.Date <= toDate);

        if (type.HasValue)
            query = query.Where(p => p.Type == type);

        var payments = query
            .OrderByDescending(p => p.Date)
            .ToList();

        return View(payments);
    }
}
