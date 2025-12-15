namespace LogisticsApp.Models;

public class User
{
    public enum UserRole
    {
        User,
        Admin
    }

    public Guid UserId { get; set; } = new Guid();
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
}