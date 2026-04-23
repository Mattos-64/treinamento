namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public IList<Book> Books { get; set; } = new List<Book>(); // Um autor tem vários livros.
    }
}
    