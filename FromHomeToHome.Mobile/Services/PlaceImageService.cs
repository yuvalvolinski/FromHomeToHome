using System.Net.Http.Json;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Mobile.Config;

namespace FromHomeToHome.Mobile.Services;

public interface IPlaceImageService
{
    Task<RandomPlaceImageDto?> GetRandomImageAsync();
}

public class PlaceImageService : IPlaceImageService
{
    private readonly HttpClient _httpClient;

    public PlaceImageService()
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(handler);
    }

    public async Task<RandomPlaceImageDto?> GetRandomImageAsync()
    {
        var response = await _httpClient.GetAsync(ApiConfig.RandomPlaceImageEndpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var image = await response.Content.ReadFromJsonAsync<RandomPlaceImageDto>();
        if (image == null)
        {
            return null;
        }

        if (!Uri.TryCreate(image.ImageUrl, UriKind.Absolute, out _))
        {
            image.ImageUrl = $"{ApiConfig.BaseUrl}{image.ImageUrl}";
        }

        return image;
    }
}
