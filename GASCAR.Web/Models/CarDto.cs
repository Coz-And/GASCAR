namespace GASCAR.Web.Models;

public class CarDto
{
    public int Id { get; set; }
    public string Model { get; set; } = "";
    public double CurrentChargePercent { get; set; }
    public double TargetChargePercent { get; set; }
    public double BatteryCapacityKw { get; set; }
}
