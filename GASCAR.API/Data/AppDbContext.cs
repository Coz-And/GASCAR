using Microsoft.EntityFrameworkCore;
using GASCAR.API.Models;


namespace GASCAR.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<ChargingRequest> ChargingRequests { get; set; }
    public DbSet<MWBot> MWBots { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<Payment> Payments { get; set; }
}
