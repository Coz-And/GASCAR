namespace GASCAR.Web.Services;

public class AuthStateService
{
    public string? Token { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public void SetToken(string token)
    {
        Token = token;
    }

    public void Logout()
    {
        Token = null;
    }
}
