namespace Gascar.Models;

public class ParkingSpot
{
    public int Id { get; set; }
    public int Number { get; set; }
    public bool IsOccupied { get; set; }

    public int? CarId { get; set; }
    public Car? Car { get; set; }
}
