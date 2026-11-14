using FromHomeToHome.Mobile.Views;

namespace FromHomeToHome.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            // Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}
