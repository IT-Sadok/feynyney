using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogisticsApp.Data;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LogisticsApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var jwt = builder.Configuration.GetSection("Jwt");

        builder.Services.AddAuthentication((options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }))
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
                };
            });
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services
            .AddIdentityCore<User>(options => { })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

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

        static string CreateJwtToken(User user, IConfiguration config)
        {
            var jwt = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
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
                    var token = CreateJwtToken(user, builder.Configuration);
                    return Results.Ok(new {token});
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

        app.MapGet("/me", [AuthorizeAttribute](ClaimsPrincipal me) =>
        {
            var userId = me.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = me.FindFirstValue(ClaimTypes.Email);

            return Results.Ok(new { userId, email });
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
        
        app.MapGet("/ping", () => Results.Ok("pong"));
        
        app.Run();
    }
}