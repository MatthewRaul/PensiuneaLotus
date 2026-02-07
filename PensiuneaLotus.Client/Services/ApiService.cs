using System.Net.Http.Json;
using PensiuneaLotus.Client.Models;
using System.Text.Json;

namespace PensiuneaLotus.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Portul tau corect din Swagger
        private const string BaseUrl = "https://localhost:7238/api/";

        public ApiService()
        {
            var handler = new HttpClientHandler();
            // Ignoram erorile de certificat pe Windows Localhost
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // --- CAMERE ---
        public async Task<List<Room>> GetRoomsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Room>>("Rooms");
        }

        // --- OASPETI (AICI ERA LIPSA) ---
        public async Task<List<Guest>> GetGuestsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Guest>>("Guests");
        }

        public async Task AddGuestAsync(Guest guest) // <--- Am adaugat metoda lipsa
        {
            await _httpClient.PostAsJsonAsync("Guests", guest);
        }

        // --- REZERVARI ---
        public async Task<Reservation> AddReservationAsync(Reservation reservation)
        {
            var response = await _httpClient.PostAsJsonAsync("Reservations", reservation);

            if (response.IsSuccessStatusCode)
            {
                // Returnam rezervarea cu ID-ul nou creat
                return await response.Content.ReadFromJsonAsync<Reservation>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        // --- SERVICII ---
        public async Task<List<Service>> GetServicesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Services");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return await response.Content.ReadFromJsonAsync<List<Service>>(options) ?? new List<Service>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare Servicii: {ex.Message}");
            }
            return new List<Service>();
        }

        // --- PLATI SI REZERVARE SERVICII ---
        public async Task AddReservationServiceAsync(ReservationService rs)
        {
            await _httpClient.PostAsJsonAsync("ReservationServices", rs);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _httpClient.PostAsJsonAsync("Payments", payment);
        }
    }
}