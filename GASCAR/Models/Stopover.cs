namespace Gascar.Models;

public class Stopover
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public required Car Car { get; set; }

    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }

    public decimal TotalCost { get; set; }
}
