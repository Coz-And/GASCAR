using GASCAR.API.Data;
using GASCAR.API.Models;
using GASCAR.API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly OpenChargeMapService _ocm;

    public StationsController(AppDbContext db, OpenChargeMapService ocm)
    {
        _db = db;
        _ocm = ocm;
    }

    [HttpGet("real")]
    public async Task<IActionResult> GetRealStations(double? latitude, double? longitude, int? distanceKm, int? maxResults, CancellationToken ct)
    {
        var lat = latitude ?? 45.4642;
        var lon = longitude ?? 9.1900;
        var dist = distanceKm ?? 10;
        var max = maxResults ?? 25;

        var pois = await _ocm.GetStationsAsync(lat, lon, dist, max, ct);
        if (pois.Count == 0) return Ok(new List<ParkingSpot>());

        var ids = pois.Select(p => p.Id).ToList();
        var existing = await _db.ParkingSpots.Where(p => ids.Contains(p.Id)).ToListAsync(ct);
        var existingById = existing.ToDictionary(p => p.Id, p => p);

        var result = new List<ParkingSpot>();
        foreach (var poi in pois)
        {
            if (!existingById.TryGetValue(poi.Id, out var spot))
            {
                spot = new ParkingSpot
                {
                    Id = poi.Id,
                    Name = poi.AddressInfo?.Title ?? $"Colonnina #{poi.Id}",
                    IsOccupied = false,
                    CurrentCarId = null
                };
                _db.ParkingSpots.Add(spot);
            }

            if (poi.AddressInfo != null)
            {
                spot.Name = poi.AddressInfo.Title ?? $"Colonnina #{poi.Id}";
                spot.Latitude = poi.AddressInfo.Latitude;
                spot.Longitude = poi.AddressInfo.Longitude;
                spot.Address = BuildAddress(poi.AddressInfo, poi.Id);
            }

            result.Add(spot);
        }

        await _db.SaveChangesAsync(ct);
        return Ok(result);
    }

    private static string BuildAddress(OpenChargeMapService.AddressInfo info, int id)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(info.Title)) parts.Add(info.Title);
        if (!string.IsNullOrWhiteSpace(info.AddressLine1)) parts.Add(info.AddressLine1);
        if (!string.IsNullOrWhiteSpace(info.Town)) parts.Add(info.Town);
        if (!string.IsNullOrWhiteSpace(info.Postcode)) parts.Add(info.Postcode);

        if (parts.Count == 0) return $"Colonnina OCM #{id}";
        return string.Join(", ", parts);
    }
}
