// See https://aka.ms/new-console-template for more information

using LibraryApp;
using LibraryApp.Managers;
using LibraryApp.Models;

FileManager fm = new FileManager();
Library lib = new Library(fm);
App app = new App(lib);
LibraryConcurrencySimulator simulator = new LibraryConcurrencySimulator(lib);

await simulator.RunUpdateSimulationAsync(100);
Console.WriteLine("After simulation: ");
foreach (var b in lib.GetBooks())
{
    Console.WriteLine($"{b.Id}: {b.Title} | {b.Author} | {b.Year}");
}

// app.RunApp();







