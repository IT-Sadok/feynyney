// See https://aka.ms/new-console-template for more information

using LibraryApp;
using LibraryApp.Managers;
using LibraryApp.Models;

Library lib = new Library();
FileManager fm = new FileManager();
App app = new App(lib, fm);

app.RunApp();







