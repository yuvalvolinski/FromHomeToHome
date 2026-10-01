using FromHomeToHome.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FromHomeToHome.Mobile
{
    internal class RegisterViewModel
    {
        public ICommand GoToLoginCommand { get; }


        public RegisterViewModel()
        {
            // קישור הפקודה לפונקציה
            GoToLoginCommand = new Command(OnGoToLogin);
        }

        private async void OnGoToLogin()
        {
            // ניווט אסינכרוני דרך ה-Shell

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            await Shell.Current.GoToAsync("..");

        }
    }
}

