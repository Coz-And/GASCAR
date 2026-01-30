using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.JSInterop;

namespace GASCAR.Web.Services;

public class AuthStateService
{
    private readonly IJSRuntime _js;

    public AuthStateService(IJSRuntime js)
    {
        _js = js;
    }

    public string? Token { get; private set; }
    public string? UserRole { get; private set; }
    public string? Username { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    public bool IsAdmin => UserRole == "Admin";

    public void SetToken(string token)
    {
        var normalized = NormalizeToken(token);
        if (string.IsNullOrEmpty(normalized)) return;

        Token = normalized;
        UserRole = ExtractRoleFromToken(normalized);
        Username = ExtractUsernameFromToken(normalized);
        _ = _js.InvokeVoidAsync("localStorage.setItem", "authToken", Token);
    }

    public async Task LoadTokenAsync()
    {
        if (!string.IsNullOrEmpty(Token)) return;

        var stored = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
        var normalized = NormalizeToken(stored);
        if (!string.IsNullOrEmpty(normalized))
        {
            Token = normalized;
            UserRole = ExtractRoleFromToken(normalized);
            Username = ExtractUsernameFromToken(normalized);
        }
    }

    private static string? NormalizeToken(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var trimmed = raw.Trim();
        if (trimmed.StartsWith("{") && trimmed.Contains("token"))
        {
            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                if (doc.RootElement.TryGetProperty("token", out var tokenProp))
                {
                    return tokenProp.GetString();
                }
            }
            catch
            {
                return null;
            }
        }

        if (trimmed.StartsWith("token:", StringComparison.OrdinalIgnoreCase))
        {
            return trimmed.Substring("token:".Length).Trim();
        }

        return trimmed;
    }

    public void Logout()
    {
        Token = null;
        UserRole = null;
        Username = null;
        _ = _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
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
