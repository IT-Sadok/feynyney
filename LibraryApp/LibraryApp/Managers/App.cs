using LibraryApp.Models;

namespace LibraryApp.Managers;

public class App
{
    private Library _lib;
    public int Choice { get; set; }
    
    public App(Library lib)
    {
        _lib = lib;
    }
    
    public void RunApp()
    {
        while (true)
        {
            PrintMainMenuInConsole();
    
            switch (MakeChoiceInConsole())
            {
                case 1:
                    ReadAndPrintAllBooksInConsole();
                    break;
                case 2:
                    AddNewBook();
                    break;
                case 3:
                    DeleteBookById();
                    break;
                case 4:
                    BorrowBook();
                    break;
                case 5: 
                    ReturnBook();
                    break;
                case 6:
                    FindBookByAuthor();
                    break;
                case 7:
                    FindBookByTitle();
                    break;
                default:
                    return;
            }
        }
    }

    public void ReadAndPrintAllBooksInConsole()
    {
        _lib.SetBooksList();
        PrintBooksListInConsole(_lib.GetBooks());
    }

    public void AddNewBook()
    {
        _lib.SetBooksList();
        
        Console.Write("Book title: ");
        string title = Console.ReadLine();

        Console.Write("Book author: ");
        string author = Console.ReadLine();

        Console.Write("Book year: ");
        int year = int.Parse(Console.ReadLine());
        
        _lib.AddNewBook(title, author, year);
    }

    public void DeleteBookById()
    {
        Console.WriteLine("Enter book ID to delete or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (int.TryParse(input, out int id))
        {
            try
            {
                _lib.RemoveBookById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
    }

    private void FindBookByAuthor()
    {
        Console.WriteLine("Enter the author to find: ");
        var input = Console.ReadLine();

        try
        {
            var result = _lib.SearchBookByAuthor(input);
            
            if (!result.Any())
            {
                Console.WriteLine("This author is not found");
            }
            else
            {
                PrintBooksListInConsole(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    
    private void FindBookByTitle()
    {
        Console.WriteLine("Enter the title to find: ");
        var input = Console.ReadLine();

        try
        {
            var result = _lib.SearchBookByTitle(input);
            
            if (!result.Any())
            {
                Console.WriteLine("This title is not found");
            }
            else
            {
                PrintBooksListInConsole(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void PrintMainMenuInConsole()
    {
        Console.WriteLine("Choose Operation: \n 1 - Read Library \n 2 - Add new book \n 3 - Delete book \n 4 - Borrow Book \n 5 - Return Book \n 6 - Search by the author \n 7 - Search by the title \n Any button to exit ");
    }

    private int MakeChoiceInConsole()
    {
        return Choice = int.Parse(Console.ReadLine());
    }

    private void BorrowBook()
    {
        Console.WriteLine("Enter book ID to borrow or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (int.TryParse(input, out int id))
        {
            try
            {
                _lib.BorrowBookById(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
    }
    
    private void ReturnBook()
    {
        Console.WriteLine("Enter book ID to return or N to cancel");
        var input = Console.ReadLine();

        if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (int.TryParse(input, out int id))
        {
            try
            {
                _lib.ReturnBookById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number or N to cancel");
        }
    }
    
    private void PrintBooksListInConsole(List<Book> books)
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