namespace GASCAR.API.Models;

public class ParkingSpot
{
    public int Id { get; set; }
    public bool IsOccupied { get; set; }
    public int? CurrentCarId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
}
