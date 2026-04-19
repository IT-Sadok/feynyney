namespace LogisticsApp.Application;

public class Routes
{
    //auth
    public const string AuthBase = "/auth";
    public const string Login = AuthBase + "/login";
    public const string Register = AuthBase + "/register";
    
    //user
    public const string Me = AuthBase + "/me";    
    
    //packages
    public const string Packages = AuthBase + "/packages";
    public const string MyPackages = "/packages/my";
    public const string PackageNumber = "/packages/track/{trackingNumber}";
}