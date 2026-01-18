using Microsoft.AspNetCore.Identity;

public enum UserRole { Admin, Automobilista }
public enum UserType { Base, Premium }

public class ApplicationUser : IdentityUser
{
    public UserRole Ruolo { get; set; }
    public UserType Tipo { get; set; }
}
