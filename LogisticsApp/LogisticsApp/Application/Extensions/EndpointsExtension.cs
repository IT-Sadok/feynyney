using LogisticsApp.Application;
using LogisticsApp.Application.Auth;
using LogisticsApp.Application.Users;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

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
             return Results.Ok(new LoginResponseModel(token));
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