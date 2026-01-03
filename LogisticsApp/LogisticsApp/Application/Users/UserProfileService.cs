using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Application.Users;

public class UserProfileService : IUserProfileService
{
    private readonly ICurrentUserService _current;
    private readonly UserManager<User> _userManager;

    public UserProfileService(ICurrentUserService current,  UserManager<User> userManager)
    {
        _current = current;
        _userManager = userManager;
    }
    
    public async Task<UserModel> GetMe()
    {
        var user = await _userManager.FindByIdAsync(_current.UserId);

        return new UserModel(user!.Id, user.Email!, user.UserName!);
    }
}