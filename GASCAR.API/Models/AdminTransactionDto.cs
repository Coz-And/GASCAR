namespace GASCAR.API.Models
{
    public class AdminTransactionDto
    {
        public string UserId { get; set; } = "";
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
    }
}
