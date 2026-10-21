namespace FromHomeToHome.Mobile.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

	private async void OnAlreadyHaveAccountClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}
