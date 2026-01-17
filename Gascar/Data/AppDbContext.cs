using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gascar.Models;

namespace Gascar.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Parking> Parking { get; set; }
    public DbSet<ChargingRequest> ChargingRequests { get; set; }
    public DbSet<Payment> Payments { get; set; }
}
