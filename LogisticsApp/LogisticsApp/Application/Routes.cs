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
    public const string Packages = "/packages";
    public const string AllPackages = "/packages/all";
    public const string MyIncomingPackages = "/packages/my/incoming";
    public const string MySentPackages = "/packages/my/sent";
    public const string PackageNumber = "/packages/track/{trackingNumber}";
    public const string ReceivePackages = "/packages/receive";
    
    //transport
    public const string Transports = "/transports";
    
    //terminal
    public const string Terminals = "/terminals";
}