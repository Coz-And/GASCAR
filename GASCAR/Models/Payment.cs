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

    public decimal Amount { get; set; }   

    public DateTime Date { get; set; }
      public required string UserId { get; set; }
    public required ApplicationUser User { get; set; }
};
public required ApplicationUser User { get; set; }

