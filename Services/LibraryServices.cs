using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class LibraryService
    {
        private readonly LibraryDataContext _context;

        public LibraryService(LibraryDataContext context)
        {
            _context = context;
        }

        public void VincularCategoriaAoLivro(int livroid, string nomeCategoria)
        {
            var livro = _context.Books
                .Include(b => b.Categories)
                .FirstOrDefault(b => b.Id == livroid);

            if (livro == null) return;
            
                var categoria = _context.Categories
                    .FirstOrDefault(c => c.Name == nomeCategoria)
                    ?? new Category { Name = nomeCategoria };

                livro.Categories.Add(categoria);
                _context.SaveChanges();

                Console.WriteLine($"Sucesso: Categoria {nomeCategoria} vinculada ao livro {livro.Title}");
            
        }
    }
}