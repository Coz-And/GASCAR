using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using GASCAR.API.Data;
using GASCAR.API.Service;
using GASCAR.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargingController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly MWBotService _mwbot;

    public ChargingController(AppDbContext db, MWBotService mwbot)
    {
        _db = db;
        _mwbot = mwbot;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestCharging(int carId, double targetPercent)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null) return NotFound();

        car.TargetChargePercent = targetPercent;

        var queueCount = await _db.ChargingRequests
            .CountAsync(r => r.Status == "Pending");

        var req = new ChargingRequest
        {
            CarId = carId,
            RequestTime = DateTime.Now,
            EstimatedWaitMinutes = queueCount * 20,
            Status = "Pending"
        };

        _db.ChargingRequests.Add(req);
        await _db.SaveChangesAsync();

        _ = _mwbot.ProcessQueue();

        return Ok(new {
            queueCount,
            req.EstimatedWaitMinutes
        });
    }

    [HttpGet("status/{carId}")]
    public async Task<IActionResult> GetStatus(int carId)
    {
        var req = await _db.ChargingRequests
            .Where(r => r.CarId == carId)
            .OrderByDescending(r => r.RequestTime)
            .FirstOrDefaultAsync();

        if (req == null) return NotFound();

        return Ok(req);
    }
}
