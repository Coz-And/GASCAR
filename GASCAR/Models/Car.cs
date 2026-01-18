public class Car
{
    public int Id { get; set; }
    public string Modello { get; set; }
    public double CapacitaBatteriaKW { get; set; }
    public int PercentualeAttuale { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
