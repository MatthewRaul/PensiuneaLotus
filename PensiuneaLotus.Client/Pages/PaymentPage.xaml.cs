using PensiuneaLotus.Client.Models;
using PensiuneaLotus.Client.Services;
using System.Collections.ObjectModel;

namespace PensiuneaLotus.Client.Pages;

public partial class PaymentPage : ContentPage
{
    private readonly ApiService _apiService;
    private Reservation _reservation;
    private Room _room;

    // Lista speciala care include proprietatea "IsSelected" pentru Checkbox
    public ObservableCollection<SelectableService> ServiceList { get; set; } = new ObservableCollection<SelectableService>();

    public PaymentPage(Reservation reservation, Room room)
    {
        InitializeComponent();
        _apiService = new ApiService();
        _reservation = reservation;
        _room = room;

        BindingContext = this;
        ColServices.ItemsSource = ServiceList;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        // 1. Calculam costul camerei
        int nights = (_reservation.CheckOutDate - _reservation.CheckInDate).Days;
        if (nights < 1) nights = 1;

        decimal roomTotal = nights * _room.PricePerNight;

        LblRoomInfo.Text = $"Camera {_room.Number} x {nights} nopți";
        LblRoomPrice.Text = $"{roomTotal} RON";

        // 2. Incarcam serviciile din API
        try
        {
            var apiServices = await _apiService.GetServicesAsync();
            ServiceList.Clear();
            foreach (var s in apiServices)
            {
                // Le transformam in SelectableService
                ServiceList.Add(new SelectableService
                {
                    ID = s.ID,
                    Name = s.Name,
                    Price = s.Price,
                    IsSelected = false
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Nu s-au putut încărca serviciile.", "OK");
        }

        UpdateTotal();
    }

    // Se apeleaza cand bifezi un serviciu
    private void OnServiceCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        // Recalculam tot
        int nights = (_reservation.CheckOutDate - _reservation.CheckInDate).Days;
        if (nights < 1) nights = 1;
        decimal roomTotal = nights * _room.PricePerNight;

        decimal servicesTotal = 0;
        foreach (var s in ServiceList)
        {
            if (s.IsSelected)
            {
                servicesTotal += s.Price;
            }
        }

        decimal grandTotal = roomTotal + servicesTotal;
        LblGrandTotal.Text = $"{grandTotal} RON";
    }

    private async void OnConfirmPaymentClicked(object sender, EventArgs e)
    {
        if (PickerPaymentMethod.SelectedItem == null)
        {
            await DisplayAlert("Eroare", "Selectează metoda de plată!", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirmare", $"Înregistrezi plata de {LblGrandTotal.Text}?", "DA", "NU");
        if (!confirm) return;

        try
        {
            // 1. Salvam serviciile selectate in baza de date
            foreach (var s in ServiceList)
            {
                if (s.IsSelected)
                {
                    var resService = new ReservationService
                    {
                        ReservationID = _reservation.ID,
                        ServiceID = s.ID,
                        Quantity = 1
                    };
                    await _apiService.AddReservationServiceAsync(resService);
                }
            }

            // 2. Salvam Plata
            // Mai intai trebuie sa extragem suma numerica din label sau sa o recalculam
            string totalString = LblGrandTotal.Text.Replace(" RON", "").Trim();
            decimal finalAmount = decimal.Parse(totalString);

            var payment = new Payment
            {
                ReservationID = _reservation.ID,
                Amount = finalAmount,
                Method = PickerPaymentMethod.SelectedItem.ToString(),
                PaidAt = DateTime.Now
            };

            await _apiService.AddPaymentAsync(payment);

            await DisplayAlert("Succes", "Plata și serviciile au fost salvate!", "OK");
            await Navigation.PopToRootAsync(); // Ne intoarcem la prima pagina
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", "Ceva nu a mers: " + ex.Message, "OK");
        }
    }
}

// Clasa ajutatoare pentru UI (Wrapper)
public class SelectableService : Service
{
    public bool IsSelected { get; set; }
}