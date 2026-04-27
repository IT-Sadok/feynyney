using LogisticsApp.DTO;

namespace LogisticsApp.Application.Auth;

public interface IAuthService
{
    Task Register(RegisterModel model, CancellationToken ct);
    Task<string> Login(LoginModel model, CancellationToken ct);
}