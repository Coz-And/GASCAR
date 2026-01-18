namespace Gascar.Models;

public enum ChargingState
{
    Waiting,
    InProgress,
    Completed
}

public class Charging
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public required Car Car { get; set; }

    public int TargetChargePercentage { get; set; }
    public ChargingState State { get; set; }

    public TimeSpan EstimatedTime { get; set; }
    public double ConsumedKw { get; set; }
}
