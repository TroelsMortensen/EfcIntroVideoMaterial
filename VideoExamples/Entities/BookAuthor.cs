namespace VideoExamples.Entities;

public class BookAuthor
{
    public int Order { get; set; }
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    
    // relationships
    public Book Book { get; set; }
    public Author Author { get; set; }
}