using System.Security.Claims;

namespace LogisticsApp.Application.Users;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http)
    {
        _http = http;
    }
    
    public string UserId =>
        _http.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();
}