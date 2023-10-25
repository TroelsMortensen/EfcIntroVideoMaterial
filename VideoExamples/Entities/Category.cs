namespace VideoExamples.Entities;

public class Category
{
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; }
}