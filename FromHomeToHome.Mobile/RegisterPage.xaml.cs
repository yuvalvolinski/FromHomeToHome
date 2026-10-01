using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();

        BindingContext = new RegisterViewModel();
    }


    
}
