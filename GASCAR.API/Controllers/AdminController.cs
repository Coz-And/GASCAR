using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GASCAR.API.Data;
using Microsoft.EntityFrameworkCore;
using GASCAR.API.Models;

namespace GASCAR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
            [HttpGet("transactions")]
            public async Task<IActionResult> GetAdminTransactions()
            {
                var transactions = await _db.Payments
                    .Join(_db.Users,
                          payment => payment.UserId,
                          user => user.Id,
                          (payment, user) => new GASCAR.Web.Models.AdminTransactionDto
                          {
                              UserId = user.Id.ToString(),
                              Date = payment.StartTime,
                              Type = payment.Type,
                              Amount = payment.Amount,
                              Status = payment.UserType // O altro campo se necessario
                          })
                    .ToListAsync();
                return Ok(transactions);
            }
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        // --- API CRUD COLONNINE (ParkingSpot) ---
        [HttpGet("stations")]
        public async Task<IActionResult> GetStations()
        {
            var stations = await _db.ParkingSpots.ToListAsync();
            return Ok(stations);
        }

        [HttpPost("stations")]
        public async Task<IActionResult> AddStation([FromBody] ParkingSpot spot)
        {
            _db.ParkingSpots.Add(spot);
            await _db.SaveChangesAsync();
            return Ok(spot);
        }

        [HttpPut("stations/{id}")]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] ParkingSpot spot)
        {
            var existing = await _db.ParkingSpots.FindAsync(id);
            if (existing == null) return NotFound();
            existing.IsOccupied = spot.IsOccupied;
            existing.CurrentCarId = spot.CurrentCarId;
            existing.Latitude = spot.Latitude;
            existing.Longitude = spot.Longitude;
            existing.Address = spot.Address;
            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("stations/{id}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var existing = await _db.ParkingSpots.FindAsync(id);
            if (existing == null) return NotFound();
            _db.ParkingSpots.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok();
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
}
