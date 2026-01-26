using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GASCAR.API.Data;
using Microsoft.EntityFrameworkCore;
using GASCAR.API.Models;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("parking-status")]
    public async Task<IActionResult> ParkingStatus()
    {
        var spots = await _db.ParkingSpots.ToListAsync();
        var mwbot = await _db.MWBots.FirstAsync();
        return Ok(new { spots, mwbot });
    }

    [HttpPut("tariffs")]
    public async Task<IActionResult> UpdateTariffs(Tariff t)
    {
        var tariff = await _db.Tariffs.FirstAsync();
        tariff.ParkingCostPerHour = t.ParkingCostPerHour;
        tariff.ChargingCostPerKw = t.ChargingCostPerKw;
        await _db.SaveChangesAsync();
        return Ok(tariff);
    }

    [HttpGet("payments")]
    public async Task<IActionResult> Payments(DateTime from, DateTime to)
    {
        var payments = await _db.Payments
            .Where(p => p.StartTime >= from && p.EndTime <= to)
            .ToListAsync();

        var totalParking = payments
            .Where(p => p.Type == "Parking")
            .Sum(p => p.Amount);

        var totalCharging = payments
            .Where(p => p.Type == "Charging")
            .Sum(p => p.Amount);

        return Ok(new { payments, totalParking, totalCharging });
    }
}
