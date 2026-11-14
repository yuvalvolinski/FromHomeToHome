using System.Windows.Input;
using FromHomeToHome.Mobile.Views;

namespace FromHomeToHome.Mobile.ViewModels;

public class MainViewModel
{
    public ICommand GoToRegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    public MainViewModel()
    {
        // קישור הפקודה לפונקציה
        GoToRegisterCommand = new Command(OnGoToRegister);
        GoToLoginCommand = new Command(OnGoToLogin);
    }

    private async void OnGoToRegister()
    {
        // ניווט אסינכרוני דרך ה-Shell

        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        await Shell.Current.GoToAsync(nameof(RegisterPage));

    }

    private async void OnGoToLogin()
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }
}