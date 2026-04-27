using LogisticsApp.DTO;

namespace LogisticsApp.Application.Users;

public interface IUserProfileService
{
    Task<UserModel> GetMe(CancellationToken ct);
}