namespace Gascar.Models;

public class Car
{
    public int Id { get; set; }

    public required string Modello { get; set; }

    public double BatteryCapacityKw { get; set; }
    public int CurrentChargePercentage { get; set; }

    // Relazione con User
    public required string UserId { get; set; }
    public required ApplicationUser User { get; set; }
}
