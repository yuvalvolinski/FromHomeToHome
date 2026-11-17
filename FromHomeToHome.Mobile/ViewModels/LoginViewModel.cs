using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Mobile.Services;

namespace FromHomeToHome.Mobile.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly IUserSessionService _sessionService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError = false;
    private bool _isLoading = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Email
    {
        get => _email;
        set { if (_email != value) { _email = value; OnPropertyChanged(); } }
    }

    public string Password
    {
        get => _password;
        set { if (_password != value) { _password = value; OnPropertyChanged(); } }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { if (_errorMessage != value) { _errorMessage = value; OnPropertyChanged(); } }
    }

    public bool HasError
    {
        get => _hasError;
        set { if (_hasError != value) { _hasError = value; OnPropertyChanged(); } }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { if (_isLoading != value) { _isLoading = value; OnPropertyChanged(); } }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel() : this(GetAuthService(), GetSessionService())
    {
    }

    public LoginViewModel(IAuthService authService, IUserSessionService sessionService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        LoginCommand = new AsyncRelayCommand(OnLogin);
        System.Diagnostics.Debug.WriteLine("LoginViewModel initialized successfully");
    }

    private static IAuthService GetAuthService()
    {
        try
        {
            var service = IPlatformApplication.Current?.Services.GetService<IAuthService>();
            if (service != null) return service;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error resolving AuthService from DI: {ex.Message}");
        }

        return new AuthService();
    }

    private static IUserSessionService GetSessionService()
    {
        try
        {
            var service = IPlatformApplication.Current?.Services.GetService<IUserSessionService>();
            if (service != null) return service;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error resolving UserSessionService from DI: {ex.Message}");
        }

        return new UserSessionService();
    }

    private async Task OnLogin()
    {
        System.Diagnostics.Debug.WriteLine("OnLogin called");

        HasError = false;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
        {
            ShowError("נא להזין כתובת אימייל.");
            return;
        }

        if (!IsValidEmail(Email))
        {
            ShowError("כתובת האימייל אינה תקינה.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ShowError("נא להזין סיסמה.");
            return;
        }

        try
        {
            IsLoading = true;

            var loginDto = new LoginDto
            {
                Email = Email.Trim(),
                Password = Password
            };

            System.Diagnostics.Debug.WriteLine($"Calling AuthService.LoginAsync for {loginDto.Email}");
            var user = await _authService.LoginAsync(loginDto);

            if (user != null)
            {
                _sessionService.SetUser(user);

                await Shell.Current.DisplayAlertAsync(
                    "ברוך הבא!",
                    $"התחברת בהצלחה, {user.Username}.",
                    "אישור");
            }
            else
            {
                ShowError("אימייל או סיסמה שגויים.");
            }
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"HttpRequestException: {ex.Message}");
            ShowError($"שגיאת חיבור. וודא שהשרת זמין ב-{Config.ApiConfig.BaseUrl}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            ShowError($"אירעה שגיאה: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = true;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (Shell.Current != null)
                await Shell.Current.DisplayAlertAsync("שגיאה", message, "אישור");
        });
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
