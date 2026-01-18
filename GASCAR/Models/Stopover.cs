public class Stopsover
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public Auto Auto { get; set; }

    public DateTime OrarioIngresso { get; set; }
    public DateTime? OrarioUscita { get; set; }

    public decimal CostoTotale { get; set; }
}
