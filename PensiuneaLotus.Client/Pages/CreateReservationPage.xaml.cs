using PensiuneaLotus.Client.Models;
using PensiuneaLotus.Client.Services;

namespace PensiuneaLotus.Client.Pages;

public partial class CreateReservationPage : ContentPage
{
    private readonly ApiService _apiService;
    private List<Room> _rooms;
    private List<Guest> _guests;

    public CreateReservationPage()
    {
        InitializeComponent();
        _apiService = new ApiService();

        // Setam datele default (Azi -> Maine)
        DateCheckIn.Date = DateTime.Now;
        DateCheckOut.Date = DateTime.Now.AddDays(1);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            var allRooms = await _apiService.GetRoomsAsync();
            // Filtram doar camerele libere
            _rooms = allRooms.Where(r => r.IsOccupied == false).ToList();

            _guests = await _apiService.GetGuestsAsync();

            PickerRoom.ItemsSource = _rooms;
            PickerGuest.ItemsSource = _guests;

            // Daca nu sunt camere, afisam mesaj informativ
            if (_rooms.Count == 0)
            {
                // Putem afisa un mesaj scurt sau un Toast, dar DisplayAlert e ok aici
                // await DisplayAlert("Info", "Nu există camere libere momentan!", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Nu s-au putut incarca listele: " + ex.Message, "OK");
        }
    }

    private async void OnSaveReservationClicked(object sender, EventArgs e)
    {
        // 1. Validari
        if (PickerRoom.SelectedItem == null || PickerGuest.SelectedItem == null)
        {
            await DisplayAlert("Eroare", "Selecteaza camera si oaspetele!", "OK");
            return;
        }

        if (DateCheckOut.Date <= DateCheckIn.Date)
        {
            await DisplayAlert("Eroare", "Data de plecare trebuie sa fie dupa sosire!", "OK");
            return;
        }

        // 2. Pregatim datele
        var selectedRoom = (Room)PickerRoom.SelectedItem;
        var selectedGuest = (Guest)PickerGuest.SelectedItem;

        var reservation = new Reservation
        {
            RoomID = selectedRoom.ID,
            GuestID = selectedGuest.ID,
            CheckInDate = DateCheckIn.Date,
            CheckOutDate = DateCheckOut.Date
        };

        // 3. Trimitem la API si navigam DIRECT la Plata (Fara intreruperi)
        try
        {
            // Salvam si primim ID-ul
            var savedReservation = await _apiService.AddReservationAsync(reservation);

            // --- MODIFICARE: AM SCOS ALERT-UL DE SUCCES ---
            // Nu mai intrebam nimic, mergem direct la plata.

            // Navigare instanta
            await Navigation.PushAsync(new PaymentPage(savedReservation, selectedRoom));

            // Curatam formularul pentru cand ne intoarcem (daca utilizatorul da Back din plata)
            PickerRoom.SelectedItem = null;
            PickerGuest.SelectedItem = null;
            DateCheckIn.Date = DateTime.Now;
            DateCheckOut.Date = DateTime.Now.AddDays(1);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare API", ex.Message, "OK");
        }
    }
}