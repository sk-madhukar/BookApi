namespace BookApi.Models
{
    //public record Book(int Id, string Title, string Author)
    //{

    //}

    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; } //Why added "?" here?
        public string? Author { get; set; }
        public string? Year { get; set; }
    }
}
