using Gascar.Data;
using Gascar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gascar.Controllers;

[Authorize]
public class PaymentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PaymentsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // LISTA PAGAMENTI UTENTE
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var payments = await _context.Payments
            .Where(p => p.UserId == user!.Id)
            .OrderByDescending(p => p.Date)
            .ToListAsync();

        return View(payments);
    }

    // FORM PAGAMENTO
    public IActionResult Create()
    {
        return View();
    }

    // SALVATAGGIO PAGAMENTO
    [HttpPost]
    public async Task<IActionResult> Create(decimal amount)
    {
        var user = await _userManager.GetUserAsync(User);

        var payment = new Payment
        {
            Amount = amount,
            UserId = user!.Id
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
