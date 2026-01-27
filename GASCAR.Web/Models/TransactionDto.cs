namespace GASCAR.Web.Models;

public class TransactionDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = "";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
}