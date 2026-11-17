using Microsoft.Extensions.Logging;
using FromHomeToHome.Mobile.Services;
using FromHomeToHome.Mobile.ViewModels;

namespace FromHomeToHome.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IUserSessionService, UserSessionService>();

            // Register ViewModels
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<MainViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
