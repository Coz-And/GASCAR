using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GASCAR.Web.Services;

public class AuthStateService
{
    public string? Token { get; private set; }
    public string? UserRole { get; private set; }
    public string? Username { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    public bool IsAdmin => UserRole == "Admin";

    public void SetToken(string token)
    {
        Token = token;
        UserRole = ExtractRoleFromToken(token);
        Username = ExtractUsernameFromToken(token);
    }

    public void Logout()
    {
        Token = null;
        UserRole = null;
        Username = null;
    }

    private string? ExtractRoleFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
            return roleClaim?.Value;
        }
        catch
        {
            return null;
        }
    }

    private string? ExtractUsernameFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            // Cerca i claim comuni per username
            var usernameClaim = jwt.Claims.FirstOrDefault(c => c.Type == "unique_name" || c.Type == "name" || c.Type == "sub");
            return usernameClaim?.Value;
        }
        catch
        {
            return null;
        }
    }
}
