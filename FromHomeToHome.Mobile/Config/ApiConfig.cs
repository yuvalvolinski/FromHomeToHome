namespace FromHomeToHome.Mobile.Config;

public static class ApiConfig
{
    // Base URL of the REST API Backend
    // Local development: http://localhost:3000
    // Note: For Android emulator use http://10.0.2.2:3000
    // Production: update this to your production server
    public static string BaseUrl => "http://localhost:3000";

    public static string RegisterEndpoint => $"{BaseUrl}/api/auth/register";
    public static string LoginEndpoint => $"{BaseUrl}/api/auth/login";
}
