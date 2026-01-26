using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GASCAR.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
    private readonly AppDbContext _db;

    public ParkingController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("spots")]
    public async Task<IActionResult> GetSpots()
    {
        return Ok(await _db.ParkingSpots.ToListAsync());
    }

    [HttpPost("enter")]
    public async Task<IActionResult> Enter(int carId)
    {
        var spot = await _db.ParkingSpots.FirstOrDefaultAsync(s => !s.IsOccupied);
        if (spot == null) return BadRequest("No free spots");

        spot.IsOccupied = true;
        spot.CurrentCarId = carId;

        await _db.SaveChangesAsync();
        return Ok(spot);
    }

    [HttpPost("exit")]
    public async Task<IActionResult> Exit(int carId)
    {
        var spot = await _db.ParkingSpots
            .FirstOrDefaultAsync(s => s.CurrentCarId == carId);

        if (spot == null) return NotFound();

        spot.IsOccupied = false;
        spot.CurrentCarId = null;

        await _db.SaveChangesAsync();
        return Ok();
    }
}
