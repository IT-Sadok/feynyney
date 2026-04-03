using System.Security.Claims;
using LogisticsApp.DTO;

namespace LogisticsApp.Application.Users;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http)
    {
        _http = http;
    }


    public UserContextModel User =>
        new(
            _http.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException()
        );
}