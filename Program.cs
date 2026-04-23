using Library.Data;
using System.Linq;
using System;
using Library.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

var service = new LibraryService (new Library.Data.LibraryDataContext());

bool rodando = true;

while (rodando)
{
    Console.Clear();
    Console.WriteLine("\n ======= Sistema de gerenciamento de biblioteca ===== ");
    Console.WriteLine(" 1. Listar acervo completo.");
    Console.WriteLine(" 2. Cadastrar novo autor.");
    Console.WriteLine(" 3. Cadastrar novo livro.");
    Console.WriteLine(" 4. Vincular autor a um livro.");
    Console.WriteLine(" 5. Listar IDs ( autores e livros ).");
    Console.WriteLine(" 6. Buscar livro por título.");
    Console.WriteLine(" 7. Remover autor.");
    Console.WriteLine(" 8. Consultar orfãos.");
    Console.WriteLine(" 0. sair.");
    Console.WriteLine("\n Escolha uma das opções acima: ");
    


    string opcao = Console.ReadLine();

    switch (opcao)
    {
        // Listar acervo completo
        case "1":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            service.GerarRelatorioAcervo();
            Console.ResetColor();
            break;

        // Cadastrar novo autor
        case "2":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Nome do autor: ");
            string nome = (Console.ReadLine());

            Console.WriteLine("Bio: ");
            string bio = (Console.ReadLine());

            service.AdicionarNovoAutor(nome,bio);
            Console.ResetColor();
            break;

        // Cadastrar novo livro
        case "3":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Título do novo livro: ");
            string titulo = (Console.ReadLine());

            Console.WriteLine("ISBN: ");
            string isbn = (Console.ReadLine());

            service.AdicionarLivroSemAutor(titulo, isbn);
            Console.ResetColor();
            break;

        // Vincular autor a um livro
        case "4":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            service.ListarAutores();
            service.ListarLivros();

            Console.WriteLine("\n Digite o Id do autor: ");
            int idA = int.Parse(Console.ReadLine());

            Console.WriteLine("\n Digite o Id do livro: ");
            int  idL = int.Parse(Console.ReadLine());

            service.VincularAutorAoLivro(idA, idL);
            Console.ResetColor();
            break;

        // Listar IDs ( autores e livros )
        case "5":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            service.ListarAutores();
            service.ListarLivros();
            Console.ResetColor();
            break;

        // Buscar livro por título
        case "6":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Digite o título ( ou parte dele ) para buscar: ");
            string busca = (Console.ReadLine());
            service.BuscarLivroPorTitulo(busca);
            Console.ResetColor();
            break;

        // Remover autor
        case "7":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Digite o Id do autor que deseja remover:");
            if (int.TryParse(Console.ReadLine(), out int ida))
            {
                service.RemoverAutor(ida);
            }
            else
            {
                Console.WriteLine("[ERROR]: Por gentileza, digitar apenas números para ID.");
            }
            Console.ResetColor();
            break;

        case "8":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            service.ListarOrfaos();
            Console.ResetColor();
            break;

        // sair
        case "0":
            rodando = false;
            Console.WriteLine("Encerrando sistema... ");
            break;

        default:
            Console.WriteLine("[ERROR]: Oopção invalida ! ");
            break;
    }

    if (rodando)
    {
        Console.WriteLine("\n Pressione qualquer tecla para voltar ao menu... ");
        Console.ReadKey();
    }

}