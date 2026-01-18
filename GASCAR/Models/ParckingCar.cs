public class ParkingCar
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public bool Occupato { get; set; }

    public int? AutoId { get; set; }
    public Auto Auto { get; set; }
}
