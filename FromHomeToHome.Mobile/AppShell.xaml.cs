using FromHomeToHome.Mobile.Views;

namespace FromHomeToHome.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
            Routing.RegisterRoute(nameof(PlayPage), typeof(PlayPage));
        }
    }
}
