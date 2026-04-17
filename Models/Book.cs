namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public DateTime PublishedDate { get; set; }

        // Relacionamentos:

        public int AuthorId { get; set; }

        public Author Author { get; set; } // Um livro tem um autor ( 1:N )

        public IList<Category> Categories { get; set; } = new List<Category>(); // Um livro tem várias categorias ( N:N )

    }
}
	