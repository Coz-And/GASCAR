using System.Net.Http.Headers;
using System.Net.Http.Json;
using GASCAR.Web.Models;

namespace GASCAR.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly AuthStateService _auth;

    public ApiService(HttpClient http, AuthStateService auth)
    {
        _http = http;
        _auth = auth;
    }

    private void AttachToken()
    {
        if (!string.IsNullOrEmpty(_auth.Token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _auth.Token);
        }
    }

    public async Task<List<CarDto>> GetCars()
    {
        try
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<CarDto>>("api/cars")
                   ?? new();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore GetCars: {ex.Message}");
            return new();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout GetCars - API non risponde");
            return new();
        }
    }

    public async Task<bool> Login(LoginDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!res.IsSuccessStatusCode) return false;

            var token = await res.Content.ReadAsStringAsync();
            _auth.SetToken(token.Replace("\"", ""));
            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore Login: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout Login - API non risponde");
            return false;
        }
    }

    public async Task<List<PaymentDto>> GetPayments()
    {
        try
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<PaymentDto>>("api/payments")
                   ?? new();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore GetPayments: {ex.Message}");
            return new();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout GetPayments - API non risponde");
            return new();
        }
    }

    public async Task<bool> RequestCharging(int carId)
    {
        try
        {
            AttachToken();
            var res = await _http.PostAsync($"api/charging/{carId}", null);
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore RequestCharging: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout RequestCharging - API non risponde");
            return false;
        }
    }

    public async Task<bool> Register(LoginDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/register", dto);
            if (!res.IsSuccessStatusCode) return false;

            var token = await res.Content.ReadAsStringAsync();
            _auth.SetToken(token.Replace("\"", ""));
            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore Register: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout Register - API non risponde");
            return false;
        }
    }

    public async Task<bool> ForgotPassword(string email)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/forgot-password", new { Email = email });
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Errore ForgotPassword: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("⏱️ Timeout ForgotPassword - API non risponde");
            return false;
        }
    }
}
