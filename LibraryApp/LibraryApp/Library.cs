using LibraryApp.Models;

namespace LibraryApp;

public class Library
{
    private int _nextId;

    private List<Book> _books = new List<Book>();

    public List<Book> GetBooks()
    {
        return _books;
    }

    public void AddNewBook()
    {
        Console.WriteLine("Book title: "); 
        string title = Console.ReadLine();

        Console.WriteLine("Book author: ");
        string author = Console.ReadLine();

        Console.WriteLine("Book year: ");
        int year = int.Parse(Console.ReadLine());

        if (_books.Count > 0)
        {
            _nextId = _books.Max(b => b.Id) + 1;
        }
        else
        {
            _nextId = 1;
        }

        Book newBook = new Book()
        {
            Id = _nextId,
            Title = title,
            Author = author,
            Year = year,
            Status = Book.BookStatus.Available
        };

        _books.Add(newBook);
    }

    public List<Book> SearchBookByAuthor(string author)
    {
        var bookToFind = _books.Where(b => b.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase)).ToList();
        
        if (!bookToFind.Any()) 
        {
            Console.WriteLine("This author is not found");
        }
        
        return bookToFind;
    }
    
    public List<Book> SearchBookByTitle(string title)
    {
        var bookToFind = _books.Where(b => b.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase)).ToList();
        
        if (!bookToFind.Any()) 
        {
            Console.WriteLine("This title is not found");
        }
        
        return bookToFind;
    }

    public void BorrowBookById(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);

        if (book == null || book.Status == Book.BookStatus.Borrowed)
        {
            Console.WriteLine($"Book with id {bookId} is borrowed or doesnt exist");
        }
        else
        {
            book.Status = Book.BookStatus.Borrowed;
        }
    }

    public void ReturnBookById(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);

        if (book == null || book.Status == Book.BookStatus.Available)
        {
            Console.WriteLine($"Book with id {bookId} is already available");
        }
        else
        {
            book.Status = Book.BookStatus.Available;
        }
    }

    public void SetBooksList(List<Book> books)
    {
        if (books.Count > 0)
        {
            _books = books;
        }
        else
        {
            Console.WriteLine("No books to add to list");
        }
    }

    public void RemoveBookById(int bookId)
    {
        var bookToDelete = _books.FirstOrDefault(b => b.Id == bookId);

        if (bookToDelete != null)
        {
            _books.Remove(bookToDelete);
        }
        else
        {
            Console.WriteLine($"Book with id {bookId} is not found");
        }
    }
}