using GASCAR.API.Models;
using BCrypt.Net;

namespace GASCAR.API.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {

        // Cancella tutti gli utenti normali (non admin)
        var normalUsers = db.Users.Where(u => u.Role != "Admin").ToList();
        if (normalUsers.Any())
        {
            db.Users.RemoveRange(normalUsers);
            db.SaveChanges();
        }

        // Seed Admin User
        if (!db.Users.Any(u => u.Email == "admin@gascar.com"))
        {
            var adminUser = new User
            {
                Email = "admin@gascar.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                UserType = "Administrator"
            };
            db.Users.Add(adminUser);
        }

        // Seed utente base predefinito
        if (!db.Users.Any(u => u.Email == "utente@gascar.com"))
        {
            var baseUser = new User
            {
                Email = "utente@gascar.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Utente123!"),
                Role = "User",
                UserType = "Base"
            };
            db.Users.Add(baseUser);
        }

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
