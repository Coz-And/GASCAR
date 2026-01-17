using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Gascar.Pages;
public class RegisterModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string? ConfirmPassword { get; set; }

    public string? ErrorMessage { get; set; }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Compila tutti i campi";
            return Page();
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Le password non coincidono";
            return Page();
        }

        // TODO: salva utente
        return RedirectToPage("/Login");
    }
}

