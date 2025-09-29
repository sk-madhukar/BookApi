using BookApi.Models;

namespace BookApi.Services
{
    public class BookService
    {
        private readonly List<Book> _books = new()
        {
            new Book(1, "Clean Code", "Robert C. Martin"),
            new Book(2, "The Pragmatic Programmer", "Andrew Hunt")
        };

        public IEnumerable<Book> GetAll() => _books;
        public Book? GetById(int id) => _books.FirstOrDefault(x => x.Id == id);
        public void Add(Book book) => _books.Add(book);
        public void Update(int id, Book updatedBook)
        {
            var index = _books.FindIndex(b => b.Id == id);
            if(index != -1)
            {
                _books[index] = updatedBook;
            }
        }
        public void Delete(int id) => _books.RemoveAll(b => b.Id == id);
    }
}
