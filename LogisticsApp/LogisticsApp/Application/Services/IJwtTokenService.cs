using LogisticsApp.DTO;
using LogisticsApp.Models;

namespace LogisticsApp.Services;

public interface IJwtTokenService
{
    string CreateToken(UserTokenModel  model);
}