using FromHomeToHome.Core.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;
using MauiMap = Microsoft.Maui.Controls.Maps.Map;
using MauiMapSpan = Microsoft.Maui.Maps.MapSpan;
using MauiMapType = Microsoft.Maui.Maps.MapType;
using MauiDistance = Microsoft.Maui.Maps.Distance;

namespace FromHomeToHome.Mobile.Views;

public partial class MapModalPage : ContentPage
{
    private readonly MauiMap _map;
    private Pin? _selectedPin;

    public MapModalPage(Level level)
    {
        Title = "Map";
        FlowDirection = FlowDirection.RightToLeft;
        BackgroundColor = Color.FromArgb("#0F172A");

        var closeButton = new Button
        {
            Text = "סגור",
            BackgroundColor = Color.FromArgb("#38BDF8"),
            TextColor = Color.FromArgb("#0F172A"),
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 12,
            HeightRequest = 44,
            HorizontalOptions = LayoutOptions.End
        };
        closeButton.Clicked += OnCloseClicked;

        _map = new MauiMap(
            MauiMapSpan.FromCenterAndRadius(
                new MauiLocation(level.MapLatitude, level.MapLongitude),
                MauiDistance.FromKilometers(level.MapRadiusKilometers)))
        {
            MapType = MauiMapType.Street,
            IsShowingUser = false
        };
        _map.MapClicked += OnMapClicked;

        var mapBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#1E293B"),
            Stroke = Color.FromArgb("#334155"),
            StrokeThickness = 1,
            Content = _map
        };

        var layout = new Grid
        {
            Padding = 16,
            RowSpacing = 12,
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            }
        };

        layout.Children.Add(closeButton);
        layout.Children.Add(mapBorder);
        Grid.SetRow(mapBorder, 1);

        Content = layout;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnMapClicked(object? sender, MapClickedEventArgs e)
    {
        if (_selectedPin != null)
        {
            _map.Pins.Remove(_selectedPin);
        }

        _selectedPin = new Pin
        {
            Label = "הנקודה שנבחרה",
            Location = e.Location
        };

        _map.Pins.Add(_selectedPin);

        await DisplayAlert(
            "קואורדינטות נבחרו",
            $"Latitude: {e.Location.Latitude:F6}\nLongitude: {e.Location.Longitude:F6}",
            "OK");
    }
}