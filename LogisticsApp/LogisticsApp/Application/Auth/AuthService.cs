using FluentValidation;
using LogisticsApp.Application.Validation;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using LogisticsApp.Services;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwt;
    private readonly IValidator<RegisterModel> _registerValidator;
    private readonly IValidator<LoginModel> _loginValidator;

    public AuthService(
        UserManager<User> userManager,
        IJwtTokenService jwt,
        IValidator<RegisterModel> registerValidator,
        IValidator<LoginModel> loginValidator)
    {
        _userManager = userManager;
        _jwt = jwt;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }
    
    public async Task Register(RegisterModel model, CancellationToken ct)
    {
        //validation
        await _registerValidator.ValidateAndThrowAsync(model, cancellationToken: ct);
        
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if(!result.Succeeded)
            throw new ApplicationException("User creation failed");
    }

    public async Task<string> Login(LoginModel model, CancellationToken ct)
    {
        await _loginValidator.ValidateAndThrowAsync(model, ct);
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException();
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var tokenUser = new UserTokenModel(
            user.Id,
            user.Email ?? "",
            user.UserName ?? "",
            roles.ToList()
            );

        return _jwt.CreateToken(tokenUser);
    }
}