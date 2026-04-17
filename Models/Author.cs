namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public IList<Book> Books { get; set; } // Um autor tem vários livros.
    }
}
    