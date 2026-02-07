using System.Text.RegularExpressions; // <--- Avem nevoie de asta pentru validare
using PensiuneaLotus.Client.Models;
using PensiuneaLotus.Client.Services;

namespace PensiuneaLotus.Client.Pages
{
    public partial class GuestsPage : ContentPage
    {
        private readonly ApiService _apiService;

        public GuestsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadGuests();
        }

        private async Task LoadGuests()
        {
            try
            {
                var guests = await _apiService.GetGuestsAsync();
                ColGuests.ItemsSource = guests;
            }
            catch (Exception ex)
            {
                // Ignoram erorile la incarcare sau le afisam discret
                Console.WriteLine(ex.Message);
            }
        }

        private async void OnAddGuestClicked(object sender, EventArgs e)
        {
            // 1. Validare campuri goale
            if (string.IsNullOrWhiteSpace(EntFirstName.Text) ||
                string.IsNullOrWhiteSpace(EntLastName.Text) ||
                string.IsNullOrWhiteSpace(EntPhone.Text) ||
                string.IsNullOrWhiteSpace(EntEmail.Text))
            {
                await DisplayAlert("Eroare", "Toate campurile sunt obligatorii!", "OK");
                return;
            }

            // 2. Validare Telefon (Exact ca in Backend-ul tau)
            // Regex-ul tau accepta: 0722-123-123 sau 0722.123.123 sau 0722 123 123 sau legat
            string telefon = EntPhone.Text.Trim();
            var phoneRegex = new Regex(@"^0\d{3}([ .-]?\d{3}){2}$");

            if (!phoneRegex.IsMatch(telefon))
            {
                await DisplayAlert("Format Invalid",
                    "Telefonul trebuie sa fie de forma:\n07xx 123 123 sau\n07xx-123-123 sau\n07xx123123",
                    "OK");
                return;
            }

            // 3. Validare Email
            string email = EntEmail.Text.Trim();
            if (!email.Contains("@") || !email.Contains("."))
            {
                await DisplayAlert("Email Invalid", "Adresa de email nu este corecta.", "OK");
                return;
            }

            try
            {
                var newGuest = new Guest
                {
                    FirstName = EntFirstName.Text.Trim(),
                    LastName = EntLastName.Text.Trim(),
                    Email = email,
                    Phone = telefon
                };

                // Trimitem la server
                await _apiService.AddGuestAsync(newGuest);

                await DisplayAlert("Succes", "Oaspete salvat cu succes!", "OK");

                // Golim formularul
                EntFirstName.Text = "";
                EntLastName.Text = "";
                EntEmail.Text = "";
                EntPhone.Text = "";

                // Reincarcam lista
                await LoadGuests();
            }
            catch (Exception ex)
            {
                // Aici prindem erorile de la Server (ex: email duplicat etc.)
                await DisplayAlert("Eroare Server", "Nu s-a putut salva: " + ex.Message, "OK");
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await LoadGuests();
        }
    }
}