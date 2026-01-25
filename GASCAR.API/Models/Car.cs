public class Car
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public double BatteryCapacityKw { get; set; }
    public double CurrentChargePercent { get; set; }
    public double TargetChargePercent { get; set; }
    public int UserId { get; set; }
}
