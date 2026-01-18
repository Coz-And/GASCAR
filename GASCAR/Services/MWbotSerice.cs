using Microsoft.EntityFrameworkCore;
using Gascar.Data;
using Gascar.Models;

namespace Gascar.Services
{
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
                var db = scope.ServiceProvider
                              .GetRequiredService<ApplicationDbContext>();

                var ricarica = await db.Ricariche
                    .Include(r => r.Auto)
                    .Where(r => r.Stato == StatoRicarica.InCoda)
                    .OrderBy(r => r.Id)
                    .FirstOrDefaultAsync(stoppingToken);

                if (ricarica != null)
                {
                    ricarica.Stato = StatoRicarica.InRicarica;
                    await db.SaveChangesAsync(stoppingToken);

                    await Task.Delay(ricarica.TempoStimato, stoppingToken);

                    ricarica.Stato = StatoRicarica.Completata;
                    ricarica.Auto.PercentualeAttuale = ricarica.PercentualeRichiesta;

                    await db.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
