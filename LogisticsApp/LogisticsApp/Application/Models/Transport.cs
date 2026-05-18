namespace LogisticsApp.Models;

public class Transport
{
    public int Id { get; set; }
    public string Type { get; set; } = String.Empty;
    public TransportStatus Status { get; set; }
    
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}