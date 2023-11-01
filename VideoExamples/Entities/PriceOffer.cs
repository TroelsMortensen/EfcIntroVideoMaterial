namespace VideoExamples.Entities;

public class PriceOffer
{
    public int Id { get; set; }
    public decimal PromotionalPrice { get; set; }
    public string? PromotionalText { get; set; }
    public int BookId { get; set; }
}