using GASCAR.API.Data;
using GASCAR.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GASCAR.API.Service;

public class PaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateChargingPayment(Car car)
    {
        // 🔒 Evita crash se non esiste nessuna tariffa
        var tariff = await _db.Tariffs.FirstOrDefaultAsync();
        if (tariff == null)
            return;

        // 🔒 Evita crash se l'utente non esiste
        var user = await _db.Users.FindAsync(car.UserId);
        if (user == null)
            return;

        double kwToCharge =
            (car.TargetChargePercent - car.CurrentChargePercent) / 100.0
            * car.BatteryCapacityKw;

        var cost = kwToCharge * tariff.ChargingCostPerKw;

        _db.Payments.Add(new Payment
        {
            UserId = user.Id,
            Amount = (decimal)cost,
            Type = "Charging",
            StartTime = DateTime.UtcNow.AddMinutes(-20),
            EndTime = DateTime.UtcNow,
            UserType = user.UserType
        });

        car.CurrentChargePercent = car.TargetChargePercent;

        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdatePaymentAsync(Payment payment)
    {
        var existing = await _db.Payments.FindAsync(payment.Id);
        if (existing == null)
            return false;

        existing.Amount = payment.Amount;
        existing.Type = payment.Type;
        existing.StartTime = payment.StartTime;
        existing.EndTime = payment.EndTime;
        existing.UserType = payment.UserType;
        // UserId non modificabile per sicurezza

        await _db.SaveChangesAsync();
        return true;
    }
}
