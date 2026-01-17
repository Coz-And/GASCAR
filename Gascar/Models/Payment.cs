using System.ComponentModel.DataAnnotations;

namespace Gascar.Models;

public class Payment
{
    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // Relazione con Identity
    [Required]
    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}
