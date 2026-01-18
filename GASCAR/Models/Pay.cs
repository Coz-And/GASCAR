public enum TipoPagamento { Stopsover, Charging }

public class Pay
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public TipoPagamento Tipo { get; set; }
    public decimal Importo { get; set; }
    public DateTime Data { get; set; }
}
