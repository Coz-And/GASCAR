namespace Gascar.Models
{
    public class User
    {
        public int Id { get; set; }

        public required string Username { get; set; } 
         public required string Email { get; set; } = null!;
        public required string Password { get; set; } = null!;
        public required string? Role { get; set; }// Admin | User
        public bool IsPremium { get; set; }
    }
}
