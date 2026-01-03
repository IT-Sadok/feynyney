using System.Security.Claims;

namespace LogisticsApp.Application.Users;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http)
    {
        _http = http;
    }
    
    public string UserId =>
        _http.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();
}