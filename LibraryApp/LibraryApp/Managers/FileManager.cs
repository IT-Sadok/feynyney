using System.Text.Json;
using LibraryApp.Models;

namespace LibraryApp.Managers;

public class FileManager
{
    private string _path = "LibraryData.json";
    
    public void WriteBooksToFile(List<Book> books)
    {
        string json = JsonSerializer.Serialize(books);
        File.WriteAllText(_path, json);
    }

    public List<Book> GetBooksListFromFile()
    {
        if (!File.Exists(_path))
        {
            return new List<Book>();
        }

        string json = File.ReadAllText(_path);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Book>();
        }
        
        return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
    }
}