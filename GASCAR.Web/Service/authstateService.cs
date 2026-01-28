using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GASCAR.Web.Services;

public class AuthStateService
{
    public string? Token { get; private set; }
    public string? UserRole { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    public bool IsAdmin => UserRole == "Admin";

    public void SetToken(string token)
    {
        Token = token;
        UserRole = ExtractRoleFromToken(token);
    }

    public void Logout()
    {
        Token = null;
        UserRole = null;
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
}
