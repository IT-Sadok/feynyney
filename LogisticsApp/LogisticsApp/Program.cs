using LogisticsApp.Data;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Add services to the container.
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapPost("/register", async (UserManager<User> userManager, RegisterDto dto) =>
        {
            var user = new User()
            {
                UserName = dto.Email,
                Email = dto.Email,
            };
            
            var result = await userManager.CreateAsync(user, dto.Password);
            
            if (result.Succeeded)
            {
                return Results.Ok(new {message = "User created successfully",  id = user.Id});
            }
            
            return Results.BadRequest(result.Errors.Select(e => new {e.Code, e.Description}));
        });
        
        app.MapPost("/login", async (UserManager<User> userManager, LoginDto dto) =>
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var ok = await userManager.CheckPasswordAsync(user, dto.Password);
                
                if (ok)
                {
                    return Results.Ok(new { message = "Logged in", userId = user.Id });
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

        app.MapGet("/users", async (UserManager<User> userManager) =>
        {
            var users = await userManager.Users
                .Select(u => new UserDto(
                    u.Id,
                    u.UserName!,
                    u.Email!
                    ))
                .ToListAsync();
            
            return Results.Ok(users);
        });
        
        app.Run();
    }
}