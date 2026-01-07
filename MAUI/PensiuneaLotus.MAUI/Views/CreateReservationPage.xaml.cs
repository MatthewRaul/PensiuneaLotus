using PensiuneaLotus.MAUI.Models;
using PensiuneaLotus.MAUI.Services;

namespace PensiuneaLotus.MAUI.Views;

public partial class CreateReservationPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();
    private Room _selectedRoom;

    public CreateReservationPage(Room room)
    {
        InitializeComponent();
        _selectedRoom = room;
        LabelRoomInfo.Text = $"Rezervare Camera {room.Number}";
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        try
        {
            // PAS 1: Creăm clientul în baza de date
            var newGuest = new Guest
            {
                Name = EntryName.Text,
                Phone = EntryPhone.Text
            };

            // Trimitem la API și primim ID-ul noului client
            int createdGuestId = await _apiService.AddGuestAsync(newGuest);

            if (createdGuestId > 0)
            {
                // PAS 2: Creăm rezervarea folosind ID-ul de mai sus
                var res = new Reservation
                {
                    RoomID = _selectedRoom.ID,
                    GuestID = createdGuestId,
                    CheckInDate = DateStart.Date,
                    CheckOutDate = DateEnd.Date,
                    Status = "Pendent"
                };

                bool success = await _apiService.AddReservationAsync(res);

                if (success)
                {
                    await DisplayAlert("Succes", "Rezervarea a fost trimisă și este vizibilă pentru Admin!", "OK");
                    await Navigation.PopToRootAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Eroare la sincronizare: " + ex.Message, "OK");
        }
    }
}