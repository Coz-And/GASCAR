using Gascar.Data;
using Gascar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gascar.Controllers;

[Authorize(Roles = "Automobilista")]
public class AutomobilistaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AutomobilistaController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RequestCharging(int carId, int targetPercentage)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null)
            return NotFound();

        var charging = new Charging
        {
            CarId = carId,
            Car = car,
            TargetChargePercentage = targetPercentage,
            State = ChargingState.Waiting,
            EstimatedTime = TimeSpan.FromMinutes(
                (targetPercentage - car.CurrentChargePercentage) * 2),
            ConsumedKw =
                (targetPercentage - car.CurrentChargePercentage)
                * car.BatteryCapacityKw / 100
        };

        _db.Chargings.Add(charging);
        await _db.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> ExitParking(int carId)
    {
        var stopover = await _db.Stopovers
            .FirstAsync(s => s.CarId == carId && s.ExitTime == null);

        stopover.ExitTime = DateTime.Now;

        var config = await _db.Configurations.FirstAsync();
        var hours = (stopover.ExitTime.Value - stopover.EntryTime).TotalHours;

        stopover.TotalCost =
            (decimal)hours * config.StopoverCostPerHour;

        var charging = await _db.Chargings
            .Where(c => c.CarId == carId && c.State == ChargingState.Completed)
            .OrderByDescending(c => c.Id)
            .FirstOrDefaultAsync();

        decimal chargingCost = 0;
        if (charging != null)
            chargingCost =
                (decimal)charging.ConsumedKw * config.CostPerKw;

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var payment = new Payment
        {
            Type = PaymentType.Stopover,
            Amount = stopover.TotalCost + chargingCost,
            Date = DateTime.Now,
            UserId = user.Id,
            User = user
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

    public IActionResult PaymentHistory()
    {
        var userId = _userManager.GetUserId(User);

        var payments = _db.Payments
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Date)
            .ToList();

        return View(payments);
    }
}
