using GASCAR.API.Data;
using GASCAR.API.Service;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Service;

public class MWBotService
{
    private readonly AppDbContext _db;
    private readonly PaymentService _payments;

    public MWBotService(AppDbContext db, PaymentService payments)
    {
        _db = db;
        _payments = payments;
    }

    public async Task ProcessQueue()
    {
        // 🔒 Evita crash se non esiste nessun bot
        var mwbot = await _db.MWBots.FirstOrDefaultAsync();
        if (mwbot == null || !mwbot.IsAvailable)
            return;

        var next = await _db.ChargingRequests
            .Where(r => r.Status == "Pending")
            .OrderBy(r => r.RequestTime)
            .FirstOrDefaultAsync();

        if (next == null)
            return;

        mwbot.IsAvailable = false;
        mwbot.CurrentCarId = next.CarId;
        next.Status = "Charging";

        await _db.SaveChangesAsync();

        await Task.Delay(TimeSpan.FromMinutes(next.EstimatedWaitMinutes));

        next.Status = "Completed";
        next.EndTime = DateTime.UtcNow;
        mwbot.IsAvailable = true;
        mwbot.CurrentCarId = null;

        if (next.StationId.HasValue)
        {
            var spot = await _db.ParkingSpots.FirstOrDefaultAsync(s => s.Id == next.StationId.Value);
            if (spot != null)
            {
                spot.IsOccupied = false;
                spot.CurrentCarId = null;
            }
        }

        // 🔧 FIX: car può essere null
        var car = await _db.Cars.FindAsync(next.CarId);
        if (car != null)
        {
            await _payments.CreateChargingPayment(car);
        }

        await _db.SaveChangesAsync();
    }
}
