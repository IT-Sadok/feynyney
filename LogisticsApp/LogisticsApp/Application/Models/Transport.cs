namespace LogisticsApp.Models;

public class Transport
{
    public int Id { get; set; }
    public string Type { get; set; } = String.Empty;
    public string Status { get; set; } = String.Empty;
    
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}