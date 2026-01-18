using Gascar.Data;
using Gascar.Models;
using Microsoft.EntityFrameworkCore;

namespace Gascar.Services;

public class MWbotService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MWbotService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var charging = await db.Chargings
                .Include(c => c.Car)
                .Where(c => c.State == ChargingState.Waiting)
                .OrderBy(c => c.Id)
                .FirstOrDefaultAsync(stoppingToken);

            if (charging != null)
            {
                charging.State = ChargingState.InProgress;
                await db.SaveChangesAsync(stoppingToken);

                await Task.Delay(charging.EstimatedTime, stoppingToken);

                charging.State = ChargingState.Completed;
                charging.Car.CurrentChargePercentage =
                    charging.TargetChargePercentage;

                await db.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
