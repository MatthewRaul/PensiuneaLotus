namespace PensiuneaLotus.MAUI.Services;

using System.Net.Http.Json;
using PensiuneaLotus.MAUI.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        // Ignorăm erorile de certificat SSL pentru mediul de dezvoltare (localhost)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(handler);

        // Setăm prefixul adresei o singură dată aici (asigură-te că portul 7073 este cel din browser)
        _httpClient.BaseAddress = new Uri("https://localhost:7073/api/");
    }

    // --- METODE PENTRU CAMERE (ROOMS) ---

    // Obține lista de camere pentru ca userul să poată alege
    public async Task<List<Room>> GetRoomsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Room>>("rooms");
    }

    // --- METODE PENTRU REZERVĂRI (RESERVATIONS) ---

    // Aceasta este metoda nouă care trimite rezervarea făcută de user către baza de date
    public async Task<bool> AddReservationAsync(Reservation reservation)
    {
        var response = await _httpClient.PostAsJsonAsync("reservations", reservation);
        return response.IsSuccessStatusCode;
    }

    public async Task<int> AddGuestAsync(Guest guest)
    {
        var response = await _httpClient.PostAsJsonAsync("guests", guest);
        if (response.IsSuccessStatusCode)
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var createdGuest = await response.Content.ReadFromJsonAsync<Guest>(options);
            // var createdGuest = await response.Content.ReadFromJsonAsync<Guest>();
            return createdGuest.ID; // Returnăm ID-ul creat de baza de date
        }
        return 0;
    }
}