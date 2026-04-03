namespace LogisticsApp.Models;

public class Terminal
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Address { get; set; } = "";
    
    public ICollection<Package> OriginPackages { get; set; } = new List<Package>();
    public ICollection<Package> DestinationPackages { get; set; } = new List<Package>();
}