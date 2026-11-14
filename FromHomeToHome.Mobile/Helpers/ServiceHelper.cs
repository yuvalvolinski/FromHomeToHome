namespace FromHomeToHome.Mobile.Helpers;

public static class ServiceHelper
{
    public static T GetService<T>() where T : class
    {
        return IPlatformApplication.Current!.Services.GetService<T>() 
            ?? throw new Exception($"Service {typeof(T).Name} not registered");
    }
}
