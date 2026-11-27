using FromHomeToHome.Core.Models;
using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile.Views;

public partial class PlayPage : ContentPage
{
    private static readonly Level CurrentLevel = new()
    {
        LevelNumber = 1,
        Name = "Israel",
        MapLatitude = 31.5,
        MapLongitude = 34.9,
        MapRadiusKilometers = 180
    };
    private readonly PlayViewModel _viewModel;

    public PlayPage()
    {
        InitializeComponent();
        _viewModel = IPlatformApplication.Current?.Services.GetService<PlayViewModel>() ?? new PlayViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private async void OnBackToGameClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnShowMapClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MapModalPage(CurrentLevel));
    }
}
