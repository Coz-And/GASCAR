namespace GASCAR.Web.Models;

public class ChargingRequestDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int StationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime RequestTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int EstimatedWaitMinutes { get; set; }
}
