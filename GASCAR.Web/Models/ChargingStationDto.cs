namespace GASCAR.Web.Models;

public class ChargingStationDto
{
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";
    public string Location { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}