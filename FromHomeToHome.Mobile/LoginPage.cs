using Microsoft.Maui.Controls.Shapes;

namespace FromHomeToHome.Mobile.Views;

public class LoginPage : ContentPage
{
    public LoginPage()
    {
        Title = "כניסה למשחק";
        NavigationPage.SetHasNavigationBar(this, false);

        var emailEntry = new Entry
        {
            Placeholder = "דואר אלקטרוני",
            Keyboard = Keyboard.Email,
            PlaceholderColor = Color.FromArgb("#64748B"),
            TextColor = Colors.White
        };

        var passwordEntry = new Entry
        {
            Placeholder = "סיסמה",
            IsPassword = true,
            PlaceholderColor = Color.FromArgb("#64748B"),
            TextColor = Colors.White
        };

        var registerButton = new Button
        {
            Text = "אין לך חשבון? הירשם כאן",
            FontSize = 13,
            TextColor = Color.FromArgb("#38BDF8"),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };
        registerButton.Clicked += OnRegisterClicked;

        var backButton = new Button
        {
            Text = "חזרה",
            FontSize = 13,
            TextColor = Color.FromArgb("#CBD5E1"),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center
        };
        backButton.Clicked += OnBackClicked;

        var content = new VerticalStackLayout
        {
            Spacing = 16,
            Padding = 30,
            Children =
            {
                new Label
                {
                    Text = "ברוכים הבאים",
                    FontSize = 28,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = "התחבר לחשבון שלך",
                    FontSize = 14,
                    TextColor = Color.FromArgb("#94A3B8"),
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 15)
                },
                CreateField(emailEntry),
                CreateField(passwordEntry),
                new Button
                {
                    Text = "התחבר",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    BackgroundColor = Color.FromArgb("#2563EB"),
                    CornerRadius = 25,
                    HeightRequest = 52,
                    Margin = new Thickness(0, 10, 0, 0)
                },
                registerButton,
                backButton
            }
        };

        Content = new Grid
        {
            Children =
            {
                new Image { Source = "mainwallp.png", Aspect = Aspect.AspectFill },
                new BoxView { BackgroundColor = Color.FromArgb("#0F172A"), Opacity = 0.85 },
                new ScrollView { VerticalOptions = LayoutOptions.Center, Content = content }
            }
        };
    }

    private static Border CreateField(Entry entry)
    {
        return new Border
        {
            Stroke = Color.FromArgb("#334155"),
            BackgroundColor = Color.FromArgb("#1E293B"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
            Padding = new Thickness(12, 0),
            Content = entry
        };
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}

