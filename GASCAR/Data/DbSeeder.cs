using Gascar.Models;
using Microsoft.AspNetCore.Identity;

namespace Gascar.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // RUOLI
        string[] roles = { "Admin", "Automobilista" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        // ADMIN
        var adminEmail = "admin@gascar.it";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // CONFIGURAZIONE
        if (!context.Configurations.Any())
        {
            context.Configurations.Add(new Configuration
            {
                CostPerKw = 0.30m,
                StopoverCostPerHour = 2.00m
            });
        }

        // PARKING SPOTS
        if (!context.ParkingSpots.Any())
        {
            for (int i = 1; i <= 10; i++)
            {
                context.ParkingSpots.Add(new ParkingSpot
                {
                    Number = i,
                    IsOccupied = false
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
