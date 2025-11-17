namespace LibraryApp.Models;

public class Book
{
    public enum BookStatus
    {
        Available,
        Borrowed
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public BookStatus Status { get; set; }
}