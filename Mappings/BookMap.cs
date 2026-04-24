using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Mappings
{

    // Código chamado de Fluent API no EF Core. É o mapa que diz ao C# exatamente como transformar sua classe Book em uma tabela física no SQL Server


    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd() // O próprio banco gera o valor.
                .UseIdentityColumn(); // No SQL Server, isso cria o 'IDENTITY (1, 1)'

            builder.Property(c => c.Title)
                .IsRequired() // NOT NULL
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR") // Permite caracteres especiais.
                .HasMaxLength(100);

            builder.Property(c => c.Isbn)
                 .IsRequired() // NOT NULL
                 .HasColumnName("Isbn")
                 .HasColumnType("VARCHAR") // Apenas letras e números, sem caracteres especiais.
                 .HasMaxLength(13);

            builder.HasIndex(c => c.Isbn, "IX_Book_Isbn") // O BD  impede que dois livros tenham o mesmo ISBN.
                .IsUnique();

            builder.Property(c => c.PublishedDate)
                .IsRequired()
                .HasColumnName("PublishedDate")
                .HasColumnType("DATETIME");

            builder.HasOne(x => x.Author) // 1 livro tem 1 autor.
                .WithMany(x => x.Books) // 1 autor tem vários livros.
                .HasConstraintName("FK_Book_Author") // Nome da FK ( Chave estrangeira )
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o autor deleta os livros

            builder.HasMany(c => c.Categories)  // 1 livro tem várias categorias.
                 .WithMany(c => c.Books) // uma categoria tem vários livros.
                 .UsingEntity<Dictionary<string, object>>( 
                     "BookCategory", // Nome da tabela de ligação que será criada no banco.
                     category => category
                         .HasOne<Category>()
                         .WithMany()
                         .HasForeignKey("CategoryId"), // Chave estrangeira para a tabela Category
                     book => book
                         .HasOne<Book>()
                         .WithMany()
                         .HasForeignKey("BookId") // FK para a tabla Book
                 ); 
        } 
    } 
} 