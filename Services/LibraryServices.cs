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

        public void GerarRelatorioAcervo()
        {
            var livros = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .ToList();

            Console.WriteLine("\n======================================");
            Console.WriteLine("     Relatório Geral da Biblioteca    ");
            Console.WriteLine("======================================\n");

            if (!livros.Any())
            {
                Console.WriteLine("O acervo está vazio no momento.");
            }

            foreach (var livro in livros)
            {
                var NomesCategorias = livro.Categories.Any()
                    ? string.Join(", ", livro.Categories.Select(c => c.Name))
                    : "Sem categorias definidas.";

                Console.WriteLine($"Livro: {livro.Title.ToUpper()}");
                Console.WriteLine($"Autor: {livro.Author?.Name ?? "Não informado"}");
                Console.WriteLine($"ISBN: {livro.Isbn}");
                Console.WriteLine($"Tags: [{NomesCategorias}]");
                Console.WriteLine("--------------------------------");
            }
        }

        public void AdicionarNovoLivro(string titulo, string isbn, int autorid, List<string> nomescategorias)
        {
            try
            {
                var NovoLivro = new Book
                {
                    Title = titulo,
                    Isbn = isbn,
                    AuthorId = autorid,
                    PublishedDate = DateTime.Now
                };

                foreach (var nome in nomescategorias)
                {
                    var cat = _context.Categories.FirstOrDefault(c => c.Name == nome)
                        ?? new Category { Name = nome };
                    NovoLivro.Categories.Add(cat);
                }

                _context.Books.Add(NovoLivro);
                _context.SaveChanges();
                Console.WriteLine($"[SUCESSO]: '{titulo}' adicionado com sucesso !!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: {ex.Message}");
            }
        }

        public void AdicionarLivroSemAutor(string titulo, string isbn)
        {
            try
            {
                var NovoLivro = new Book
                {
                    Title = titulo,
                    Isbn = isbn,
                    PublishedDate = DateTime.Now
                };
                _context.Books.Add(NovoLivro);
                _context.SaveChanges();
                Console.WriteLine($"[SUCESSO]: Livro {titulo} (!! SEM AUTOR VINCULADO !!) adicionado!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: {ex.Message}");
            }
        }

        public void AdicionarNovoAutor(string nome, string bio)
        {
            try
            {
                var NovoAutor = new Author { Name = nome, Bio = bio };
                _context.Authors.Add(NovoAutor);
                _context.SaveChanges();
                Console.WriteLine($" !! [SUCESSO]: Autor '{nome}' cadastrado !!");
            }
            catch (Exception ex)
            {
                var erro = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"[ERROR]: {erro}");
            }
        }

        public void VincularAutorAoLivro(int livroid, int autorid)
        {
            try
            {
                var livro = _context.Books.Find(livroid);
                var autor = _context.Authors.Find(autorid);

                if (livro == null || autor == null)
                {
                    Console.WriteLine("\n[ERROR]: IDs não encontrados.");
                    return;
                }

                livro.AuthorId = autorid;
                _context.SaveChanges();
                Console.WriteLine($"\n[SUCESSO]: '{livro.Title}' vinculado a '{autor.Name}'!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: {ex.Message}");
            }
        }

        public void ListarAutores()
        {
            var autores = _context.Authors.ToList();
            Console.WriteLine("\n --- Lista de Autores --- ");
            foreach (var a in autores) Console.WriteLine($"[ID]: {a.Id} | [Nome]: {a.Name}");
        }

        public void ListarLivros()
        {
            var livros = _context.Books.ToList();
            Console.WriteLine("\n --- Lista de Livros --- ");
            foreach (var l in livros) Console.WriteLine($"[ID]: {l.Id} | [Título]: {l.Title}");
        }

        public void VincularCategoriaAoLivro(int livroid, string nomeCategoria)
        {
            try
            {
                var livro = _context.Books.Include(b => b.Categories).FirstOrDefault(b => b.Id == livroid);
                if (livro == null)
                {
                    Console.WriteLine("[AVISO]: Livro não encontrado !!");
                    return;
                } 

                var categoria = _context.Categories
                    .FirstOrDefault(c => string.Equals(c.Name, nomeCategoria, StringComparison.OrdinalIgnoreCase))
                    ?? new Category { Name = nomeCategoria };

                if (livro.Categories.Any(c => c.Name.ToLower() == nomeCategoria.ToLower()))
                {
                    Console.WriteLine($"[AVISO]: O livro já possui a categoria '{nomeCategoria}'.");
                    return;
                }

                livro.Categories.Add(categoria);
                _context.SaveChanges();
                Console.WriteLine($"Sucesso: Categoria {nomeCategoria} vinculada!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: Verifique a existência da tabela de ligação. Código do erro:{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public void BuscarLivroPorTitulo(string termoBusca)
        {
            var resultados = _context.Books
                .Include(b => b.Author)
                .Where(b => b.Title.ToLower().Contains(termoBusca.ToLower()))
                .ToList();

            Console.WriteLine($"\n ----- Resultado para: '{termoBusca}' ----- ");

            if (!resultados.Any())
            {
                Console.WriteLine("Nenhum título encontrado. ");
                return;
            }

            foreach (var livro in resultados)
            {
                Console.WriteLine($"[ID]: {livro.Id}");
                Console.WriteLine($"[Título]: {livro.Title}");
                Console.WriteLine($"[Autor]: {livro.Author?.Name ?? "Sem autor"} ");
                Console.WriteLine($"[ISBN]: {livro.Isbn}");
            }
            
        }

        public void RemoverAutor(int id)
        {
            var autor = _context.Authors.Find(id);

            if (autor == null)
            {
                Console.WriteLine("[ERROR]: Autor não encontrado !!");
                return;
            }

            bool temLivroVinculado = _context.Books.Any(b => b.AuthorId == id);

            if (temLivroVinculado)
            {
                Console.WriteLine("\n [AVISO DE SEGURANÇA !!]");
                Console.WriteLine($"Não foi possível remover '{autor.Name}' pois existem livros vinculados a ele.");
                Console.WriteLine("[DICA]: remova ou altere o autor dos livros antes de excluir um autor.");
                return;
            }

            try
            {
                _context.Authors.Remove(autor);
                _context.SaveChanges();
                Console.WriteLine("Autor removido com sucesso !! ");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: Erro inesperado ocorrido: {ex.Message}");
            }

        }

        public void ListarOrfaos()
        {
            Console.WriteLine(" ===== Relatório de Registros sem Vínculo ===== ");

            // livro sem autor
            var livroSemAutor = _context.Books
                .Where(b => b.AuthorId == null)
                .ToList();

            if (livroSemAutor.Any())
            {
                foreach (var livro in livroSemAutor)
                {
                    Console.WriteLine($" [ID]: {livro.Id} | [Título]: {livro.Title}.");
                }
            }
            else
            {
                Console.WriteLine("Nenhum livro orfão encontrado !!");
            }

            // Autor sem livro
            var autorSemLivro = _context.Authors
                .Where(a => !a.Books.Any())
                .ToList();

            Console.WriteLine("\n ----- Autores sem nenhum livro no acervo ----- ");
            if (autorSemLivro.Any())
            {
                foreach (var autor in autorSemLivro)
                {
                    Console.WriteLine($"[ID]: {autor.Id} | [NOME]: {autor.Name}");
                }
            }
            else
            {
                Console.WriteLine("Todos os autores já possuem livro vinculado. ");
            }
        }

    }
}