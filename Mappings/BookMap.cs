using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Mappings
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(c => c.Title)
                .IsRequired() // NOT NULL
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(c => c.Isbn)
                 .IsRequired() // NOT NULL
                 .HasColumnName("Isbn")
                 .HasColumnType("VARCHAR")
                 .HasMaxLength(13);

            builder.HasIndex(c => c.Isbn, "IX_Book_Isbn")
                .IsUnique();

            builder.Property(c => c.PublishedDate)
                .IsRequired()
                .HasColumnName("PublishedDate")
                .HasColumnType("DATETIME");

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasConstraintName("FK_Book_Author")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Categories) 
                 .WithMany(c => c.Books)
                 .UsingEntity<Dictionary<string, object>>( 
                     "BookCategory",
                     category => category
                         .HasOne<Category>()
                         .WithMany()
                         .HasForeignKey("CategoryId"),
                     book => book
                         .HasOne<Book>()
                         .WithMany()
                         .HasForeignKey("BookId")
                 ); 
        } 
    } 
} 