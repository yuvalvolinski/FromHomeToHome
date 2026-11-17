using FromHomeToHome.Core.DTOs;

namespace FromHomeToHome.Mobile.Services;

public interface IUserSessionService
{
    LoginResponseDto? CurrentUser { get; }
    bool IsLoggedIn { get; }
    void SetUser(LoginResponseDto user);
    void Clear();
}

public class UserSessionService : IUserSessionService
{
    public LoginResponseDto? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public void SetUser(LoginResponseDto user) => CurrentUser = user;
    public void Clear() => CurrentUser = null;
}
