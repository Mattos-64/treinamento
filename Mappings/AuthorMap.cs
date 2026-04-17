using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Mappings
{
    public class AuthorMap : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            // 1. Tabela
            //Define o nome da tabela no SQL Server
            builder.ToTable("Author");

            // 2. Chave Primária
            builder.HasKey(x => x.Id);

            // 3. Identity (Auto-Incremento)
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // 4. Propriedades
            builder.Property(x => x.Name)
                .IsRequired() // NOT NULL
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            // 5. Relacionamento ( 1 para muitos )
            // Um autor tem muitos livros (Books )
            // Um livro tem apenas um autor.

            builder.HasMany(x => x.Books)
                .WithOne(x => x.Author)
                .HasConstraintName("FK_Author_Books")
                .OnDelete(DeleteBehavior.Cascade);
        }  
    }
}
