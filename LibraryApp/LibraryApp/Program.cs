// See https://aka.ms/new-console-template for more information

using LibraryApp;
using LibraryApp.Managers;
using LibraryApp.Models;

FileManager fm = new FileManager();
Library lib = new Library(fm);
App app = new App(lib);

app.RunApp();







