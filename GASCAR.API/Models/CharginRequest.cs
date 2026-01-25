public class ChargingRequest
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public required string Status { get; set; }
    public DateTime RequestTime { get; set; }
    public int EstimatedWaitMinutes { get; set; }
}
