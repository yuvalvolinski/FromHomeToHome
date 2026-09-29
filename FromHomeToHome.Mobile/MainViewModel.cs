using System.Windows.Input;
using FromHomeToHome.Mobile.Views;

namespace FromHomeToHome.Mobile.ViewModels;

public class MainViewModel
{
    public ICommand GoToRegisterCommand { get; }

    public MainViewModel()
    {
        // קישור הפקודה לפונקציה
        GoToRegisterCommand = new Command(OnGoToRegister);
    }

    private async void OnGoToRegister()
    {
        // ניווט אסינכרוני דרך ה-Shell

        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        await Shell.Current.GoToAsync(nameof(RegisterPage));

    }
}