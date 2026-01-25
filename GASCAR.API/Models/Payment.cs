public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public required string Type { get; set; } // Parking | Charging
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string UserType { get; set; }
}
