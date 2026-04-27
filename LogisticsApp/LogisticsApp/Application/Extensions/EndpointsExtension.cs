using LogisticsApp.Application;
using LogisticsApp.Application.Auth;
using LogisticsApp.Application.Packages;
using LogisticsApp.Application.Users;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Extensions;

public static class EndpointsExtension
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Register, async (
            IAuthService auth,
            RegisterModel model,
            CancellationToken ct) =>
        {
            await auth.Register(model, ct);
            return Results.Ok();
        });
        
        app.MapPost(Routes.Login, async (
            IAuthService auth,
            LoginModel model,
            CancellationToken ct) =>
        {
             var token = await auth.Login(model, ct);
             return Results.Ok(new LoginResponseModel(token));
        });

        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet(Routes.Me, [Authorize] async (
            IUserProfileService profile,
            CancellationToken ct) =>
        {
            return Results.Ok(await profile.GetMe(ct));
        });
        
        return app;
    }
}