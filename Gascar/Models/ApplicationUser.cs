using Microsoft.AspNetCore.Identity;
namespace Gascar.Models;
public class ApplicationUser : IdentityUser
{
    public string Role { get; set; } // "Admin" o "Driver"
}
