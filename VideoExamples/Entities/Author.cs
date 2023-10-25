namespace VideoExamples.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // relationships
    public ICollection<BookAuthor> BooksLink { get; set; }
}