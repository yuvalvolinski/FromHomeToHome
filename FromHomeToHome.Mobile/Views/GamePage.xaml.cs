namespace FromHomeToHome.Mobile.Views;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
	}

	private async void OnPlayClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(PlayPage));
	}
}
