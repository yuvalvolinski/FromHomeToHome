using System.Net.Http.Json;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Mobile.Config;

namespace FromHomeToHome.Mobile.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService()
    {
        System.Diagnostics.Debug.WriteLine("AuthService: Creating HttpClient");

        // Create HttpClient with certificate validation disabled for development
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _httpClient = new HttpClient(handler);

        System.Diagnostics.Debug.WriteLine($"AuthService initialized. API Endpoint: {ApiConfig.RegisterEndpoint}");
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: Starting registration for {registerDto.Username}");
            System.Diagnostics.Debug.WriteLine($"Posting to: {ApiConfig.RegisterEndpoint}");

            var response = await _httpClient.PostAsJsonAsync(
                ApiConfig.RegisterEndpoint,
                registerDto);

            System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: Response status = {response.StatusCode}");

            // Check for redirects
            if ((int)response.StatusCode == 307 || (int)response.StatusCode == 308)
            {
                System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: Redirect detected!");
                System.Diagnostics.Debug.WriteLine($"Location header: {response.Headers.Location}");
            }

            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("AuthService.RegisterAsync: Registration successful");
                return true;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: Registration failed - {content}");
                return false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: Exception - {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"AuthService.RegisterAsync: StackTrace - {ex.StackTrace}");
            return false;
        }
    }
}

