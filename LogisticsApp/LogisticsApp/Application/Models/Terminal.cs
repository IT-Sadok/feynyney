namespace LogisticsApp.Models;

public class Terminal
{
    public int Id { get; set; }
    public string Number { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    
    public ICollection<Package> OriginPackages { get; set; } = [];
    public ICollection<Package> DestinationPackages { get; set; } = [];
}