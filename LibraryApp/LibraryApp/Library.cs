using LibraryApp.Managers;
using LibraryApp.Models;

namespace LibraryApp;

public class Library
{
    private FileManager _fm;
    
    private int _nextId;

    private List<Book> _books = new List<Book>();

    public Library(FileManager fm)
    {
        _fm = fm;
        SetBooksList();
    }

    public List<Book> GetBooks()
    {
        return new List<Book>(_books);
    }

    public void AddNewBook(string title, string author, int year)
    {
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
        
        _fm.WriteBooksToFile(_books);
    }

    public List<Book> SearchByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new Exception("Author cannot be empty");
        }
        
        var foundBook = _books.Where(b => b.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase)).ToList();
        
        return foundBook;
    }
    
    public List<Book> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new Exception("Title cannot be empty");
        }
        
        var foundBook = _books.Where(b => b.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase)).ToList();
        
        return foundBook;
    }

    public void BorrowBook(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            throw new Exception("Book with this ID does not exist");
        }
        if (book.Status == Book.BookStatus.Borrowed)
        {
            throw new Exception("Book with this ID is already borrowed");
        }

        book.Status = Book.BookStatus.Borrowed;
        _fm.WriteBooksToFile(GetBooks());

    }

    public void ReturnBook(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            throw new Exception("Book with this ID does not exist");
        }
        if (book.Status == Book.BookStatus.Available)
        {
            throw new Exception("Book with this ID is already available");
        }

        book.Status = Book.BookStatus.Available;
        _fm.WriteBooksToFile(GetBooks());
    }

    private void SetBooksList()
    {
        _books = _fm.GetBooksListFromFile();
    }

    public void DeleteBook(int bookId)
    {
        var bookToDelete = _books.FirstOrDefault(b => b.Id == bookId);

        if (bookToDelete != null)
        {
            _books.Remove(bookToDelete);
            _fm.WriteBooksToFile(_books);
        }
        else
        {
            throw new Exception("Book with this ID does not exist");
        }
    }
}