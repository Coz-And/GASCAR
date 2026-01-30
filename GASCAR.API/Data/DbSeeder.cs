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
        User? baseUser = null;
        if (!db.Users.Any(u => u.Email == "utente@gascar.com"))
        {
            baseUser = new User
            {
                Email = "utente@gascar.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Utente123!"),
                Role = "User",
                UserType = "Base"
            };
            db.Users.Add(baseUser);
            db.SaveChanges(); // Salva per ottenere l'ID
        }
        else
        {
            baseUser = db.Users.FirstOrDefault(u => u.Email == "utente@gascar.com");
        }

        // Seed Auto per utente base
        if (baseUser != null && !db.Cars.Any(c => c.UserId == baseUser.Id))
        {
            var testCar = new Car
            {
                UserId = baseUser.Id,
                Model = "Tesla Model 3",
                BatteryCapacityKw = 60,
                CurrentChargePercent = 45,
                TargetChargePercent = 80
            };
            db.Cars.Add(testCar);
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

        db.SaveChanges();
    }
}
