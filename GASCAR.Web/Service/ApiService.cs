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
        AttachToken();
        return await _http.GetFromJsonAsync<List<CarDto>>("api/cars")
               ?? new();
    }

    public async Task<bool> Login(LoginDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", dto);
        if (!res.IsSuccessStatusCode) return false;

        var token = await res.Content.ReadAsStringAsync();
        _auth.SetToken(token.Replace("\"", ""));
        return true;
    }

    public async Task<List<PaymentDto>> GetPayments()
    {
        AttachToken();
        return await _http.GetFromJsonAsync<List<PaymentDto>>("api/payments")
               ?? new();
    }

    public async Task<bool> RequestCharging(int carId)
    {
        AttachToken();
        var res = await _http.PostAsync($"api/charging/{carId}", null);
        return res.IsSuccessStatusCode;
    }
}
