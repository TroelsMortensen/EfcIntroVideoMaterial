namespace VideoExamples.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string VoterName { get; set; }
    public string Comment { get; set; }
    public int BookId { get; set; }
}