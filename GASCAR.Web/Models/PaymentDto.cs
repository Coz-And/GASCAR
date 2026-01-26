namespace GASCAR.Web.Models;

public class PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
