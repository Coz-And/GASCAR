using Gascar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gascar.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Charging> Chargings { get; set; }
    public DbSet<Stopover> Stopovers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configuration
    modelBuilder.Entity<Configuration>()
        .Property(c => c.CostPerKw)
        .HasPrecision(10, 2);

    modelBuilder.Entity<Configuration>()
        .Property(c => c.StopoverCostPerHour)
        .HasPrecision(10, 2);

    // Payment
    modelBuilder.Entity<Payment>()
        .Property(p => p.Amount)
        .HasPrecision(10, 2);

    // Stopover
    modelBuilder.Entity<Stopover>()
        .Property(s => s.TotalCost)
        .HasPrecision(10, 2);
}

}



