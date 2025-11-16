using LibraryApp.Models;

namespace LibraryApp.Managers;

public class App
{
    private Library _lib;
    private FileManager _fm;
    public int Choice { get; set; }
    
    public App(Library lib, FileManager fm)
    {
        _lib = lib;
        _fm = fm;
    }
    
    public void RunApp()
    {
        while (true)
        {
            MainMenuConsole();
    
            switch (MakeChoice())
            {
                case 1:
                    UserPrintAllBooks();
                    break;
                case 2:
                    UserAddNewBook();
                    break;
                case 3:
                    UserDeleteBook();
                    break;
                case 4:
                    BorrowBook();
                    break;
                case 5: 
                    ReturnBook();
                    break;
                default:
                    return;
            }
        }
    }

    public void UserPrintAllBooks()
    {
        _lib.SetBooksList(_fm.GetBooksListFromFile());
        ConsolePrintBooksList(_lib.GetBooks());
    }

    public void UserAddNewBook()
    {
        _lib.SetBooksList(_fm.GetBooksListFromFile());
        _lib.AddNewBook();
        _fm.WriteBooksToFile(_lib.GetBooks());
    }

    public void UserDeleteBook()
    {
        Console.WriteLine("Enter book ID to delete or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (int.TryParse(input, out int id))
        {
            _lib.RemoveBookById(id);
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
        _fm.WriteBooksToFile(_lib.GetBooks());
    }

    public void MainMenuConsole()
    {
        Console.WriteLine("Choose Operation: \n 1 - Read Library \n 2 - Add new book \n 3 - Delete book \n 4 - Borrow Book \n 5 - Return Book \n Any button to exit ");
    }

    public int MakeChoice()
    {
        return Choice = int.Parse(Console.ReadLine());
    }

    public void BorrowBook()
    {
        Console.WriteLine("Enter book ID to borrow or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (int.TryParse(input, out int id))
        {
            _lib.BorrowBookById(id);
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
        
        _fm.WriteBooksToFile(_lib.GetBooks());
    }
    
    public void ReturnBook()
    {
        Console.WriteLine("Enter book ID to return or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (int.TryParse(input, out int id))
        {
            _lib.ReturnBookById(id);
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
        
        _fm.WriteBooksToFile(_lib.GetBooks());
    }
    
    public void ConsolePrintBooksList(List<Book> books)
    {
        if (books.Count != 0)
        {
            foreach (var book in books)
            {
                Console.WriteLine($"Book id: {book.Id}, title: {book.Title}, author: {book.Author}, year: {book.Year}, status : {book.Status}");
            }
        }
        else
        {
            Console.WriteLine("The list is empty");
        }
    }
}