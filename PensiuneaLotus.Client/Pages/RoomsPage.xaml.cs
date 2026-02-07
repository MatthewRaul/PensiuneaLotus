using PensiuneaLotus.Client.Services;

namespace PensiuneaLotus.Client.Pages;

public partial class RoomsPage : ContentPage
{
    private readonly ApiService _apiService;

    public RoomsPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadRooms();
    }

    private async Task LoadRooms()
    {
        try
        {
            var rooms = await _apiService.GetRoomsAsync();
            ColRooms.ItemsSource = rooms;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Nu pot incarca camerele. " + ex.Message, "OK");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadRooms();
    }
}