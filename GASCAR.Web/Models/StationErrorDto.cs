namespace GASCAR.Web.Models;

public class StationErrorDto
{
    public int Id { get; set; }
    public string StationName { get; set; } = "";
    public string ErrorType { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DetectedAt { get; set; }
    public bool IsResolved { get; set; }
}
