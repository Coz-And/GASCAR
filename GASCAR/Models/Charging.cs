
public enum StatoRicarica
{
    InCoda,
    InRicarica,
    Completata
}

public class Charging
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public Auto Auto { get; set; }

    public int PercentualeRichiesta { get; set; }
    public StatoRicarica Stato { get; set; }

    public TimeSpan TempoStimato { get; set; }
    public double KWConsumati { get; set; }
}
