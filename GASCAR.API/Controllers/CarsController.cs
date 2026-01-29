using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GASCAR.API.Data;
using GASCAR.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarsController : ControllerBase
{
    private readonly AppDbContext _db;
    public CarsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetCars()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();
        
        var cars = await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
        return Ok(cars);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromBody] Car car)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();
        
        car.UserId = userId;
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return Ok(car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
    {
        var existing = await _db.Cars.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Model = car.Model;
        existing.CurrentChargePercent = car.CurrentChargePercent;
        existing.TargetChargePercent = car.TargetChargePercent;
        existing.BatteryCapacityKw = car.BatteryCapacityKw;
        await _db.SaveChangesAsync();
        return Ok(existing);
    }
}
