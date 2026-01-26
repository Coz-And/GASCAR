using GASCAR.API.Models;

namespace GASCAR.API.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // Evita di seedare se i dati esistono già
        if (db.MWBots.Any() || db.Tariffs.Any() || db.ParkingSpots.Any())
            return;

        // Seed MWBot (Mobile Waiter Bot)
        var mwbot = new MWBot
        {
            Id = 1,
            IsAvailable = true,
            CurrentCarId = null
        };
        db.MWBots.Add(mwbot);

        // Seed Tariffs
        var tariff = new Tariff
        {
            Id = 1,
            ParkingCostPerHour = 5.00d,
            ChargingCostPerKw = 0.35d
        };
        db.Tariffs.Add(tariff);

        // Seed 20 Parking Spots
        for (int i = 1; i <= 20; i++)
        {
            db.ParkingSpots.Add(new ParkingSpot
            {
                Id = i,
                IsOccupied = false,
                CurrentCarId = null
            });
        }

        db.SaveChanges();
    }
}
