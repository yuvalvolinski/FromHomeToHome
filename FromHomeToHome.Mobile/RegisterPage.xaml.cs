using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel();
        System.Diagnostics.Debug.WriteLine("RegisterPage initialized with RegisterViewModel");
    }

    private async void OnAlreadyHaveAccountClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
