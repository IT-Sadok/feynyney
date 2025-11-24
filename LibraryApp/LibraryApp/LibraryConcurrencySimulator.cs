namespace LibraryApp;

public class LibraryConcurrencySimulator
{
    private Library _library;
    
    public LibraryConcurrencySimulator(Library library)
    {
        _library = library;
    }
    
    private object _lock = new object();

    public async Task RunUpdateSimulationAsync(int taskCount)
    {
        Task[] tasks = new Task[taskCount];
        Random rnd = new Random();

        for (int i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(() => 
            {
                int id = rnd.Next(1, _library.GetBooks().Count + 1);
                int year = rnd.Next(1800, 2026);
                    
                try
                {
                    _library.UpdateBook(id, null, null, year);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e); 
                }
            });
        }
        
        await Task.WhenAll(tasks);
    }
}