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
    private readonly PaymentService _payments;

    public ChargingController(AppDbContext db, MWBotService mwbot, PaymentService payments)
    {
        _db = db;
        _mwbot = mwbot;
        _payments = payments;
    }

    [Authorize]
    [HttpPost("request")]
    public async Task<IActionResult> RequestCharging(int carId, double targetPercent, int? stationId)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null) return NotFound();

        ParkingSpot? spot = null;
        if (stationId.HasValue && stationId.Value > 0)
        {
            spot = await _db.ParkingSpots.FirstOrDefaultAsync(s => s.Id == stationId.Value);
            if (spot == null) return NotFound("Stazione non trovata");
            if (spot.IsOccupied) return BadRequest("Stazione già occupata");
        }
        else
        {
            spot = await _db.ParkingSpots.FirstOrDefaultAsync(s => !s.IsOccupied);
            if (spot == null) return BadRequest("Nessuna colonnina disponibile");
        }

        car.TargetChargePercent = targetPercent;

        var queueCount = await _db.ChargingRequests
            .CountAsync(r => r.Status == "Pending");

        var req = new ChargingRequest
        {
            CarId = carId,
            StationId = spot?.Id,
            RequestTime = DateTime.Now,
            EstimatedWaitMinutes = queueCount * 20,
            Status = "Pending"
        };

        if (spot != null)
        {
            spot.IsOccupied = true;
            spot.CurrentCarId = carId;
        }

        _db.ChargingRequests.Add(req);
        await _db.SaveChangesAsync();

        _ = _mwbot.ProcessQueue();

        return Ok(new {
            queueCount,
            req.EstimatedWaitMinutes,
            stationId = req.StationId
        });
    }

    [Authorize]
    [HttpPost("stop")]
    public async Task<IActionResult> StopCharging(int carId)
    {
        var req = await _db.ChargingRequests
            .Where(r => r.CarId == carId && r.Status != "Completed")
            .OrderByDescending(r => r.RequestTime)
            .FirstOrDefaultAsync();

        if (req == null) return NotFound();

        req.Status = "Completed";
        req.EndTime = DateTime.UtcNow;

        if (req.StationId.HasValue)
        {
            var spot = await _db.ParkingSpots.FirstOrDefaultAsync(s => s.Id == req.StationId.Value);
            if (spot != null)
            {
                spot.IsOccupied = false;
                spot.CurrentCarId = null;
            }
        }

        var mwbot = await _db.MWBots.FirstOrDefaultAsync(b => b.CurrentCarId == carId);
        if (mwbot != null)
        {
            mwbot.IsAvailable = true;
            mwbot.CurrentCarId = null;
        }

        var car = await _db.Cars.FindAsync(carId);
        if (car != null)
        {
            await _payments.CreateChargingPayment(car);
        }

        await _db.SaveChangesAsync();
        return Ok(req);
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var carIds = await _db.Cars
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .ToListAsync();

        if (!carIds.Any()) return NotFound();

        var req = await _db.ChargingRequests
            .Where(r => carIds.Contains(r.CarId) && r.Status != "Completed")
            .OrderByDescending(r => r.RequestTime)
            .FirstOrDefaultAsync();

        if (req == null) return NotFound();

        return Ok(req);
    }

    [Authorize]
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
