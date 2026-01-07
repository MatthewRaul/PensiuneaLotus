using PensiuneaLotus.MAUI.Models;
using PensiuneaLotus.MAUI.Services;

namespace PensiuneaLotus.MAUI.Views;

public partial class RoomsPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public RoomsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            // Încărcăm camerele de la API
            var rooms = await _apiService.GetRoomsAsync();
            RoomsList.ItemsSource = rooms;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Nu s-au putut încărca camerele: " + ex.Message, "OK");
        }
    }

    private async void OnBookClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        // Preluăm obiectul Room datorită CommandParameter="{Binding .}"
        var selectedRoom = button?.CommandParameter as Room;

        if (selectedRoom != null)
        {
            // Navigăm către pagina de rezervare
            await Navigation.PushAsync(new CreateReservationPage(selectedRoom));
        }
    }
}