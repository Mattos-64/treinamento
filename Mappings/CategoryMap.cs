using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(c => c.Name)
                .IsRequired() // NOT NULL
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.HasMany(c => c.Books)
                .WithMany(c => c.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryBook", 
                    book => book.HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId"),
                    category => category.HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                );
        }
    }
}