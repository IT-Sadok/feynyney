using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Models;

public class User : IdentityUser
{ 
    public ICollection<Package> SentPackages { get; set; } = new List<Package>();
    public ICollection<Package> ReceivedPackages { get; set; } = new List<Package>();
}