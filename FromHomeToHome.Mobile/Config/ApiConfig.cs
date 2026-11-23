namespace FromHomeToHome.Mobile.Config;

public static class ApiConfig
{
    // Base URL of the REST API Backend
    // Local development (Windows): http://localhost:3000
    // Android emulator: http://10.0.2.2:3000
    // iOS simulator / Windows machine can keep localhost
    public static string BaseUrl => DeviceInfo.Platform == DevicePlatform.Android
        ? "http://10.0.2.2:3000"
        : "http://localhost:3000";

    public static string RegisterEndpoint => $"{BaseUrl}/api/auth/register";
    public static string LoginEndpoint => $"{BaseUrl}/api/auth/login";
    public static string RandomPlaceImageEndpoint => $"{BaseUrl}/api/place-images/random";
}
