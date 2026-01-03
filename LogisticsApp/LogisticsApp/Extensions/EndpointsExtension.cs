using System.Security.Claims;
using LogisticsApp.Application;
using LogisticsApp.Application.Auth;
using LogisticsApp.Application.Users;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using LogisticsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Extensions;

public static class EndpointsExtension
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Register, async (IAuthService auth, RegisterModel model) =>
        {
            await auth.Register(model);
            return Results.Ok();
        });
        
        app.MapPost(Routes.Login, async (IAuthService auth, LoginModel model) =>
        {
             var token = await auth.Login(model);
             return Results.Ok(new {token});
        });

        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet(Routes.Me, [Authorize] async (
            IUserProfileService profile) =>
        {
            return Results.Ok(await profile.GetMe());
        });
        
        return app;
    }
}