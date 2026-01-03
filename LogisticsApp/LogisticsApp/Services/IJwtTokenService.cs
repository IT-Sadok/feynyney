using LogisticsApp.Models;

namespace LogisticsApp.Services;

public interface IJwtTokenService
{
    string CreateToken(User user);
}