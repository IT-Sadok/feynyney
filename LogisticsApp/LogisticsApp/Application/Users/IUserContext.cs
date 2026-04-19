using LogisticsApp.DTO;

namespace LogisticsApp.Application.Users;

public interface IUserContext
{
    public UserContextModel User { get; }
}