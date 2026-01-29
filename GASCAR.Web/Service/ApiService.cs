using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using GASCAR.Web.Models;

namespace GASCAR.Web.Services
{

    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly AuthStateService _auth;

        public ApiService(HttpClient http, AuthStateService auth)
        {
            _http = http;
            _auth = auth;
        }

        public async Task<bool> UpdatePayment(PaymentDto payment)
        {
            AttachToken();
            var res = await _http.PutAsJsonAsync($"api/payments/{payment.Id}", payment);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> CreatePayment(PaymentDto payment)
        {
            try
            {
                AttachToken();
                var res = await _http.PostAsJsonAsync("api/payments", payment);
                return res.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Errore CreatePayment: {ex.Message}");
                return false;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("⏱️ Timeout CreatePayment - API non risponde");
                return false;
            }
        }

        private void AttachToken()
        {
            if (!string.IsNullOrEmpty(_auth.Token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _auth.Token);
            }
        }

        public async Task<bool> UpdateCar(CarDto car)
        {
            AttachToken();
            var res = await _http.PutAsJsonAsync($"api/cars/{car.Id}", car);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> CreateCar(CarDto car)
        {
            try
            {
                AttachToken();
                var res = await _http.PostAsJsonAsync("api/cars", car);
                return res.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Errore CreateCar: {ex.Message}");
                return false;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("⏱️ Timeout CreateCar - API non risponde");
                return false;
            }
        }

        // --- CRUD COLONNINE (ParkingSpot) ---
        public async Task<List<ChargingStationDto>> GetStations()
        {
            try
            {
                AttachToken();
                var result = await _http.GetFromJsonAsync<List<ChargingStationDto>>("api/admin/stations");
                return result ?? new();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Errore GetStations: {ex.Message}");
                return new();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("⏱️ Timeout GetStations - API non risponde");
                return new();
            }
        }

        public async Task<List<ParkingSpotDto>> GetPublicStations()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<ParkingSpotDto>>("api/parking/spots");
                return result ?? new();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Errore GetPublicStations: {ex.Message}");
                return new();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("⏱️ Timeout GetPublicStations - API non risponde");
                return new();
            }
        }

        public async Task<ChargingStationDto?> AddStation(ChargingStationDto station)
        {
            AttachToken();
            var res = await _http.PostAsJsonAsync("api/admin/stations", station);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<ChargingStationDto>();
        }

        public async Task<ChargingStationDto?> UpdateStation(int id, ChargingStationDto station)
        {
            AttachToken();
            var res = await _http.PutAsJsonAsync($"api/admin/stations/{id}", station);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<ChargingStationDto>();
        }

        public async Task<bool> DeleteStation(int id)
        {
            AttachToken();
            var res = await _http.DeleteAsync($"api/admin/stations/{id}");
            return res.IsSuccessStatusCode;
        }

        // --- PAGAMENTI/TRANSAZIONI ADMIN ---
        public async Task<List<PaymentDto>> GetAllPayments(DateTime? from = null, DateTime? to = null)
        {
            AttachToken();
            string url = "api/admin/payments";
            if (from != null && to != null)
                url += $"?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
            var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return new();
            var obj = await res.Content.ReadFromJsonAsync<PaymentsResponse>();
            return obj?.payments ?? new();
        }

        public class PaymentsResponse
        {
            public List<PaymentDto> payments { get; set; } = new();
            public decimal totalParking { get; set; }
            public decimal totalCharging { get; set; }
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

        public async Task<bool> Register(RegisterDto dto)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/register", dto);
                return res.IsSuccessStatusCode;
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

        public async Task<List<TransactionDto>> GetUserTransactions()
        {
            try
            {
                AttachToken();
                var response = await _http.GetFromJsonAsync<List<TransactionDto>>("api/transactions/user");
                return response ?? new List<TransactionDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Errore GetUserTransactions: {ex.Message}");
                return new List<TransactionDto>();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("⏱️ Timeout GetUserTransactions - API non risponde");
                return new List<TransactionDto>();
            }
        }

        public async Task<List<AdminTransactionDto>> GetAllAdminTransactions()
        {
            AttachToken();
            var result = await _http.GetFromJsonAsync<List<AdminTransactionDto>>("api/admin/transactions");
            return result ?? new();
        }
    }
}
