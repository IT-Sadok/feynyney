using LogisticsApp.DTO;

namespace LogisticsApp.Application.Auth;

public interface IAuthService
{
    Task Register(RegisterModel model);
    Task<string> Login(LoginModel model);
}