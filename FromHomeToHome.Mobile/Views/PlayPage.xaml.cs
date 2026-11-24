using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile.Views;

public partial class PlayPage : ContentPage
{
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
}
