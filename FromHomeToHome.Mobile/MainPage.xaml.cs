using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}