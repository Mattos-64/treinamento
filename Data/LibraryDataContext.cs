using Library.Models;
using Library.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibraryDataContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.ApplyConfiguration(new AuthorMap());
            modelbuilder.ApplyConfiguration(new CategoryMap());
            modelbuilder.ApplyConfiguration(new BookMap());
        }
    }
}