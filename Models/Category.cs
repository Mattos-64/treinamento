namespace Library.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Book> Books { get; set; } // Uma categoria tem vários livros
    }
}
    