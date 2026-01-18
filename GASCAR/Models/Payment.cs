namespace Gascar.Models;

public enum PaymentType
{
    Stopover,
    Charging
}

public class Payment
{
    public int Id { get; set; }

    public PaymentType Type { get; set; }

    public decimal Amount { get; set; }   // ⬅️ CAMBIATO

    public DateTime Date { get; set; }
}
