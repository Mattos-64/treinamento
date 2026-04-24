using Library.Validators;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using FluentValidation;

namespace Library.Services
{
    public class LibraryService
    {
        private readonly LibraryDataContext _context;
        private readonly IValidator<Book> _validator;

        public LibraryService(LibraryDataContext context, IValidator<Book> validator)
        {
            _context = context;
            _validator = validator;
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
            var NovoLivro = new Book
            {
                Title = titulo,
                Isbn = isbn,
                AuthorId = autorid,
                PublishedDate = DateTime.Now,
                Categories = new List<Category>()
            };

            //Usando o FluentValidation 
            var resultado = _validator.Validate(NovoLivro);

            if (!resultado.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" \n [ERROR DE VALIDAÇÃO]: ");

                foreach (var erro in resultado.Errors)
                {
                    Console.WriteLine($"{erro.ErrorMessage} ");
                }
                Console.ResetColor();
                return;
            }
            try
            {
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

        public void ExibirDashboard()
        {
            Console.WriteLine(" === 📊 Dashboard Estatístico da Biblioteca ===== ");

            // total de acervo
            var totalLivros = _context.Books.Count();
            var totalAutores = _context.Authors.Count();

            // Autor com mais livros no sistema
            var autorDestaque = _context.Authors
                .Select(a => new { a.Name, QtdLivros = a.Books.Count })
                .OrderByDescending(x => x.QtdLivros)
                .FirstOrDefault();

            // Categoria mais usada
            var categoriaPopular = _context.Categories
                .Select(c => new {c.Name, QtdLivros = c.Books.Count } )
                .OrderByDescending(x => x.QtdLivros)
                .FirstOrDefault();


            //Exibindo os dados
            Console.WriteLine($" \n Geral ");
            Console.WriteLine($" [Total de Livros Cadastrados]: {totalLivros} ");
            Console.WriteLine($" [Total de Autores Parceiros]: {totalAutores} ");

            Console.WriteLine($" \n Destaques ");

            if (autorDestaque != null && autorDestaque.QtdLivros > 0)
                Console.WriteLine($" Autor com mais acervo: {autorDestaque.Name} ({autorDestaque.QtdLivros} livros.)");

            if (categoriaPopular != null && categoriaPopular.QtdLivros > 0)
                Console.WriteLine($" Categoria mais presente: {categoriaPopular.Name} ({categoriaPopular.QtdLivros} livros.)");

            // Curiosidade: Livros recentes (últimos 3 cadastrados)
            var recentes = _context.Books.OrderByDescending(b => b.Id).Take(3).ToList();
            Console.WriteLine("\n Últimos lançamentos.");
            recentes.ForEach(b => Console.WriteLine($"{b.Title}"));
        }

        public void BuscarLivroPorTermo(string termo)
        {
            var livros = _context.Books
                .Where(b => b.Title.ToLower().Contains(termo.ToLower()))
                .ToList();

            if (livros.Any())
            {
                Console.WriteLine($"\n 🔎 Resultados para {termo} :");
                foreach( var livro in livros)
                {
                    Console.WriteLine($" [ID]: {livro.Id} | [Título]: {livro.Title} | [ISBN]: {livro.Isbn}");
                }
            }
            else
            {
                Console.WriteLine($"\n [!] Nhnhum livro encontrado com o termo '{termo}'.");
            }
        }

        public void TransferirLivrosEntreAutores(int idAntigo, int idNovo)
        {
            // validar se o autor novo existe
            var autorNovo = _context.Authors.Find(idNovo);
            if (autorNovo == null)
            {
                Console.WriteLine($"[ERROR]: o autor de destino {idNovo} não existe. ");
                    return;
            }

            // Buscar todos os livros do autor antigo
            var livrosParaMover = _context.Books
                .Where(b => b.AuthorId == idAntigo)
                .ToList();

            if (!livrosParaMover.Any())
            {
                Console.WriteLine($"[AVISO]: O autor {idAntigo} não possui livros para transferir. ");
                    return;
            }

            // Atualizar cada livro
            foreach ( var livro in livrosParaMover)
            {
                livro.AuthorId = idNovo;
            }

            try
            {
                _context.SaveChanges();
                Console.WriteLine($"\n [SUCESSO]: {livrosParaMover.Count} livros transferidos para {autorNovo.Name}!");
            }

            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR]: Falha na transferência: {ex.Message}.");
            }

        }

        public void BuscaAvancada(string? titulo, string? nomeAutor)
        {
            //Query aberta ( traz tudo )
            IQueryable<Book> query = _context.Books.Include(b => b.Author);

            // caso o usuário insira o título
            if (!string.IsNullOrWhiteSpace(titulo))
                query = query.Where(b => b.Title.Contains(titulo));

            // Caso usuário digite o autor 
            if (!string.IsNullOrWhiteSpace(nomeAutor))
                query = query.Where(b => b.Author.Name.Contains(nomeAutor));

            var resultado = query.ToList();

            if (resultado.Any())
            {
                Console.WriteLine($"\n Encontrados exatamente {resultado.Count} livros: ");

                foreach (var b in resultado)
                    Console.WriteLine($"{b.Title} (Autor: {b.Author?.Name ?? "Sem autor"})");
            }
        }

        public void ExportarParaTxt()
        {
            string caminhoArquivo = "RelatorioAcervo.txt";
            var livros = _context.Books.Include(b => b.Author).ToList();

            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoArquivo))
                {
                    writer.WriteLine("====================================================");
                    writer.WriteLine($"RELATÓRIO DE ACERVO - GERADO EM: {DateTime.Now}");
                    writer.WriteLine("====================================================");
                    writer.WriteLine(string.Format("{0,-30} | {1,-20} | {2,-15}", "TÍTULO", "AUTOR", "ISBN"));
                    writer.WriteLine("----------------------------------------------------");

                    foreach(var b in livros)
                    {
                        writer.WriteLine(string.Format("{0,-30} | {1,-20} | {2,-15}",
                            b.Title.Length > 27 ? b.Title.Substring(0, 27) + "..." : b.Title,
                            b.Author?.Name ?? "Sem autor",
                            b.Isbn));
                    }
                }
                Console.WriteLine($"\n [SUCESSO]: Relatório gerado em: {Path.GetFullPath(caminhoArquivo)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: Falha ao gerar arquivo: {ex.Message}");
            }
        }        
    }
}