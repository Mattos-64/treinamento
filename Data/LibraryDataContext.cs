using Library.Models;
using Library.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{

    // Uma ponte entre o código C# e o SQL Server. Faz a tradução  dalinguagem de programação e linguagem de banco.


    public class LibraryDataContext : DbContext
    {
        // Cada DbSet representa uma tabela no BD.
        // Quando damos o comando: _context.Books.ToList(), o EF Core olha para o DbSet<Book> e entende que precisa lançar um SELECT * FROM Book


        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Server=(localdb)\MSSQLLocalDB: É o servidor leve que vem com o Visual Studio
            // Database=LibraryDb: O nome do banco que será criado.
            // Trusted_Connection=True: Usa o seu login do Windows para autenticar (sem precisar de senha).
            // TrustServerCertificate=True: Evita erros de SSL/Certificado em ambiente local.
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

            // Aqui, pegamos as especificações feitas nos códigos AuthorMap,CategoryMap e BookMap e forçamos o Ef Core a criar os BD's do nosso jeito.
            // Com as especificações dessas classes. Sem esse comando, o EF viria a criar o Bd do jeito dele, como ele acha certo.
            modelbuilder.ApplyConfiguration(new AuthorMap());
            modelbuilder.ApplyConfiguration(new CategoryMap());
            modelbuilder.ApplyConfiguration(new BookMap());
        }
    }
}