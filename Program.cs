using Library.Data;
using System.Linq;
using System;
using Library.Services;

using var context = new LibraryDataContext();

var libraryService = new LibraryService(context);

libraryService.VincularCategoriaAoLivro(2, "Desenvolvimento de Software");
libraryService.VincularCategoriaAoLivro(2, "Carreira");

Console.WriteLine("!! Processo Finalizado !!");