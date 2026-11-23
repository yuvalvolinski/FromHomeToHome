using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FromHomeToHome.Mobile.Services;

namespace FromHomeToHome.Mobile.ViewModels;

public class PlayViewModel : INotifyPropertyChanged
{
    private readonly IPlaceImageService _placeImageService;
    private string _challengeImageUrl = string.Empty;
    private string _statusMessage = "טוען תמונה אקראית...";
    private bool _isLoading;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string ChallengeImageUrl
    {
        get => _challengeImageUrl;
        set
        {
            if (_challengeImageUrl != value)
            {
                _challengeImageUrl = value;
                OnPropertyChanged();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage != value)
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand LoadRandomImageCommand { get; }

    public PlayViewModel() : this(GetPlaceImageService())
    {
    }

    public PlayViewModel(IPlaceImageService placeImageService)
    {
        _placeImageService = placeImageService ?? throw new ArgumentNullException(nameof(placeImageService));
        LoadRandomImageCommand = new AsyncRelayCommand(LoadRandomImageAsync);
    }

    public async Task InitializeAsync()
    {
        if (!string.IsNullOrWhiteSpace(ChallengeImageUrl))
        {
            return;
        }

        await LoadRandomImageAsync();
    }

    private static IPlaceImageService GetPlaceImageService()
    {
        try
        {
            var service = IPlatformApplication.Current?.Services.GetService<IPlaceImageService>();
            if (service != null)
            {
                return service;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error resolving PlaceImageService from DI: {ex.Message}");
        }

        return new PlaceImageService();
    }

    private async Task LoadRandomImageAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "טוען תמונה אקראית מהשרת...";

            var image = await _placeImageService.GetRandomImageAsync();
            if (image == null)
            {
                ChallengeImageUrl = string.Empty;
                StatusMessage = "לא התקבלה תמונה מהשרת.";
                return;
            }

            ChallengeImageUrl = image.ImageUrl;
            StatusMessage = image.Description;
        }
        catch (Exception ex)
        {
            ChallengeImageUrl = string.Empty;
            StatusMessage = $"שגיאה בטעינת התמונה: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
