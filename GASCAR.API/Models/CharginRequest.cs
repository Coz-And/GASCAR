namespace GASCAR.API.Models;

public class ChargingRequest
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int? StationId { get; set; }
    public required string Status { get; set; }
    public DateTime RequestTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int EstimatedWaitMinutes { get; set; }
}
