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
