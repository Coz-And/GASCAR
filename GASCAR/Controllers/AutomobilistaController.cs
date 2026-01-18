using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Gascar.Data;
using Gascar.Models;

namespace Gascar.Controllers
{
    [Authorize(Roles = "Automobilista")]
    public class AutomobilistaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AutomobilistaController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Automobilista/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // POST: /Automobilista/RichiediRicarica
        [HttpPost]
        public async Task<IActionResult> RichiediRicarica(int autoId, int percentuale)
        {
            var auto = await _db.Auto.FindAsync(autoId);
            if (auto == null)
                return NotFound();

            if (percentuale <= auto.PercentualeAttuale)
            {
                ModelState.AddModelError("", "Percentuale non valida");
                return RedirectToAction("Dashboard");
            }

            var kw = (percentuale - auto.PercentualeAttuale)
                     * auto.CapacitaBatteriaKW / 100;

            var ricarica = new Ricarica
            {
                AutoId = autoId,
                PercentualeRichiesta = percentuale,
                Stato = StatoRicarica.InCoda,
                TempoStimato = TimeSpan.FromMinutes(kw * 5),
                KWConsumati = kw
            };

            _db.Ricariche.Add(ricarica);
            await _db.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // POST: /Automobilista/EsciParcheggio
        [HttpPost]
        public async Task<IActionResult> EsciParcheggio(int autoId)
        {
            var sosta = _db.Soste
                .FirstOrDefault(s => s.AutoId == autoId && s.OrarioUscita == null);

            if (sosta == null)
                return NotFound();

            sosta.OrarioUscita = DateTime.Now;

            var config = _db.Configurazioni.First();
            var ore = (sosta.OrarioUscita.Value - sosta.OrarioIngresso).TotalHours;

            sosta.CostoTotale = (decimal)ore * config.CostoOraSosta;

            await _db.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
