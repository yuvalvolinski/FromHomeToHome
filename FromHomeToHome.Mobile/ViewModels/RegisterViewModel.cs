using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Mobile.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace FromHomeToHome.Mobile.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError = false;
    private bool _isLoading = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Username
    {
        get => _username;
        set { if (_username != value) { _username = value; OnPropertyChanged(); } }
    }

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

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set { if (_confirmPassword != value) { _confirmPassword = value; OnPropertyChanged(); } }
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

    public ICommand RegisterCommand { get; }

    public RegisterViewModel() : this(GetAuthService())
    {
    }

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        RegisterCommand = new AsyncRelayCommand(OnRegister);
        System.Diagnostics.Debug.WriteLine("RegisterViewModel initialized successfully");
    }

    private static IAuthService GetAuthService()
    {
        try
        {
            var service = IPlatformApplication.Current?.Services.GetService<IAuthService>();
            if (service != null)
            {
                System.Diagnostics.Debug.WriteLine("AuthService resolved from DI container");
                return service;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error resolving AuthService from DI: {ex.Message}");
        }

        System.Diagnostics.Debug.WriteLine("Creating new AuthService instance");
        return new AuthService();
    }

    private async Task OnRegister()
    {
        System.Diagnostics.Debug.WriteLine("OnRegister called");

        // Reset error state
        HasError = false;
        ErrorMessage = string.Empty;

        // Validation
        if (string.IsNullOrWhiteSpace(Username))
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Username is empty");
            ShowError("נא להזין שם משתמש.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Email is empty");
            ShowError("נא להזין כתובת אימייל.");
            return;
        }

        if (!IsValidEmail(Email))
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Invalid email format");
            ShowError("כתובת האימייל אינה תקינה.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Password is empty");
            ShowError("נא להזין סיסמה.");
            return;
        }

        if (Password.Length < 6)
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Password too short");
            ShowError("הסיסמה חייבת להיות לפחות 6 תווים.");
            return;
        }

        if (Password != ConfirmPassword)
        {
            System.Diagnostics.Debug.WriteLine("Validation failed: Passwords don't match");
            ShowError("הסיסמאות אינן תואמות.");
            return;
        }

        System.Diagnostics.Debug.WriteLine("All validations passed. Attempting registration...");

        // Attempt registration
        try
        {
            IsLoading = true;

            var registerDto = new RegisterDto
            {
                Username = Username.Trim(),
                Email = Email.Trim(),
                Password = Password
            };

            System.Diagnostics.Debug.WriteLine($"Calling AuthService.RegisterAsync for {registerDto.Username}");

            bool success = await _authService.RegisterAsync(registerDto);

            System.Diagnostics.Debug.WriteLine($"AuthService returned: {success}");

            if (success)
            {
                await Shell.Current.DisplayAlertAsync(
                    "הצלחה!",
                    "ההרשמה הצליחה. כעת אתה יכול להתחבר.",
                    "אישור");
                // Navigate back to login
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ShowError("ההרשמה נכשלה. נא לנסות שוב.");
            }
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"HttpRequestException: {ex.Message}");
            ShowError("שגיאת חיבור. וודא שהשרת זמין ב-http://localhost:3000");
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
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current?.MainPage?.DisplayAlert("שגיאה", message, "אישור");
        });
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
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

// Simple AsyncRelayCommand for MVVM support
public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _isExecuting = false;

    public event EventHandler? CanExecuteChanged;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public bool CanExecute(object? parameter) => !_isExecuting;

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;

        _isExecuting = true;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        try
        {
            await _execute();
        }
        finally
        {
            _isExecuting = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
