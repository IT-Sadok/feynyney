using LogisticsApp.DTO;
using LogisticsApp.Models;
using LogisticsApp.Services;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwt;

    public AuthService(UserManager<User> userManager,  IJwtTokenService jwt)
    {
        _userManager = userManager;
        _jwt = jwt;
    }
    
    public async Task Register(RegisterModel model)
    {
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if(!result.Succeeded)
            throw new ApplicationException("User creation failed");
    }

    public async Task<string> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException();

        var tokenUser = new UserTokenModel(
            user.Id,
            user.Email ?? "",
            user.UserName ?? ""
            );

        return _jwt.CreateToken(tokenUser);
    }
}