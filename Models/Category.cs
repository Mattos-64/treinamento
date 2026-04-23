namespace Library.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IList<Book> Books { get; set; } = new List<Book>(); // Uma categoria tem vários livros
    }
}
    