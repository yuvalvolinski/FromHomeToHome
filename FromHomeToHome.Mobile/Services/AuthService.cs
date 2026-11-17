using System.Net.Http.Json;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Mobile.Config;

namespace FromHomeToHome.Mobile.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService()
    {
        System.Diagnostics.Debug.WriteLine("AuthService: Creating HttpClient");

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

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: Starting login for {loginDto.Email}");
            System.Diagnostics.Debug.WriteLine($"Posting to: {ApiConfig.LoginEndpoint}");

            var response = await _httpClient.PostAsJsonAsync(
                ApiConfig.LoginEndpoint,
                loginDto);

            System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: Response status = {response.StatusCode}");

            if ((int)response.StatusCode == 307 || (int)response.StatusCode == 308)
            {
                System.Diagnostics.Debug.WriteLine("AuthService.LoginAsync: Redirect detected!");
                System.Diagnostics.Debug.WriteLine($"Location header: {response.Headers.Location}");
            }

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: Login successful for {user?.Username}");
                return user;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: Login failed - {content}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: HttpRequestException - {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: Exception - {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"AuthService.LoginAsync: StackTrace - {ex.StackTrace}");
            return null;
        }
    }
}
