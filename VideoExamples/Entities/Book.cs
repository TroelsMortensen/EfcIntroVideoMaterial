namespace VideoExamples.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal Price { get; set; }
    public string Publisher { get; set; }

    // relationships
    public PriceOffer PriceOffer { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<BookAuthor> AuthorLinks { get; set; }
}