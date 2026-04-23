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
    Console.WriteLine(" 1. Listar acervo completo. ");
    Console.WriteLine(" 2. Cadastrar novo autor. ");
    Console.WriteLine(" 3. Cadastrar novo livro. ");
    Console.WriteLine(" 4. Vincular autor a um livro. ");
    Console.WriteLine(" 5. Listar IDs ( autores e livros ). ");
    Console.WriteLine(" 6. Buscar livro por título. ");
    Console.WriteLine(" 7. Remover autor. ");
    Console.WriteLine(" 8. Consultar orfãos. ");
    Console.WriteLine(" 9. dashboard. ");
    Console.WriteLine(" 10. Busca por título. ");
    Console.WriteLine(" 11. Transferência de livro entre autores. ");
    Console.WriteLine(" 12. Busca Avançada");
    Console.WriteLine(" 13. Exportação para Txt. ");
    Console.WriteLine(" 0. sair. ");
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
            string nome = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Bio: ");
            string bio = Console.ReadLine() ?? string.Empty;

            service.AdicionarNovoAutor(nome,bio);
            Console.ResetColor();
            break;

        // Cadastrar novo livro
        case "3":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Título do novo livro: ");
            string titulo = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("ISBN: ");
            string isbn = Console.ReadLine() ?? string.Empty;

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
            string inputAutor = Console.ReadLine() ?? "";

            Console.WriteLine("\n Digite o Id do livro: ");
            string inputLivro = Console.ReadLine() ?? "";

            if (int.TryParse(inputAutor, out int idA) && int.TryParse(inputLivro, out int idL))
            {
                service.VincularAutorAoLivro(idA, idL);
                Console.WriteLine("Comando enviado ao servidor !!");
            }

            else
            {
                Console.ForegroundColor= ConsoleColor.DarkRed;
                Console.WriteLine("\n [ERROR]: Por favor, digite apenas números válidos para Ids.");
            }
            
            Console.ResetColor();
            Console.ReadKey();
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
            string busca = Console.ReadLine() ?? string.Empty;
            service.BuscarLivroPorTitulo(busca);
            Console.ResetColor();
            break;

        // Remover autor
        case "7":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" ===== Remover Autor ===== ");
            Console.WriteLine("Digite o Id do autor que deseja remover:");

            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int ida))
            {
                service.RemoverAutor(ida);
                Console.WriteLine("[SUCESSO]: Processo de remoção finalizado com sucesso !! ");
            }
            else
            {
                Console.WriteLine("[ERROR]: Por gentileza, digitar apenas números para ID.");
            }
            Console.ResetColor();
            Console.ReadKey();
            break;

        // Lista órfãos
        case "8":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            service.ListarOrfaos();
            Console.ResetColor();
            break;

        //DashBoard
        case "9":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            service.ExibirDashboard();

            Console.ResetColor();

            Console.ReadKey();
            break;

        // Busca por título
        case "10":
            Console.Clear();
            Console.ForegroundColor= ConsoleColor.Yellow;

            Console.WriteLine(" === 🔍 BUSCA POR TÍTULO === ");
            Console.WriteLine("Digite o nome ( ou parte dele ) do livro: ");

            busca = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(busca))
            {
                service.BuscarLivroPorTermo(busca);
            }
            else
            {
                Console.WriteLine(" [ERROR]: O termo de busca não pode ser vazio. ");
            }

            Console.ResetColor();
            Console.ReadKey();
            break;

        // transferindo livros entre autores
        case "11":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(" ===== 📑 Transferência de Acervo 📑 =====");

            service.ListarAutores();

            Console.WriteLine("\n Digite o Id do autor que vai SAIR: ");
            int idDe = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("\n Digite o Id do Autor que vai Receber os livros: ");
            int idPara = int.Parse(Console.ReadLine() ?? "0");

            service.TransferirLivrosEntreAutores(idDe, idPara);
            Console.ResetColor();
            Console.ReadKey();
            break;

        // Busca avançada ( Título e Autor )
        case "12":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(" ===== Busca Avançada ( múltiplos filtros ) ===== ");
            Console.WriteLine(" !! Deixe em brnaco, para ignorar um filtro !! \n");

            Console.Write("Filtrar por títulos: ");
            string fTitulo = Console.ReadLine() ?? "";

            Console.Write("Filtrar pelo nome do autor: ");
            string fAutor = Console.ReadLine() ?? "";

            service.BuscaAvancada(fTitulo, fAutor);

            Console.ResetColor();
            Console.ReadKey();
            break;

        // Exportando para Txt
        case "13":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine(" ===== 📂 Exportanto dados  =====");

                service.ExportarParaTxt();

            Console.ResetColor();
            Console.ReadKey();
            break;


        // sair
        case "0":
            rodando = false;
            Console.WriteLine("Encerrando sistema... ");
            break;

        default:
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[ERROR]: Oopção invalida ! Escolha uma opção de 0 a 9");
            Console.ResetColor();
            Thread.Sleep(1500);
            break;
    }

    if (rodando)
    {
        Console.WriteLine("\n Pressione qualquer tecla para voltar ao menu... ");
        Console.ReadKey();
    }

}