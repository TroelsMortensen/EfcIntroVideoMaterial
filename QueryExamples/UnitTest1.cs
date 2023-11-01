using Microsoft.EntityFrameworkCore;
using VideoExamples.DataAccess;
using VideoExamples.Entities;

namespace QueryExamples;

public class UnitTest1
{
    public async Task RetrievingSingleAuthorWithFind()
    {
        await using BookAppDbContext context = new BookAppDbContext();
        Author? author = await context.Author.FindAsync(1);
    }


    public async Task RetrievingSingleAuthorWithSingle()
    {
        await using BookAppDbContext context = new();
        Author author1 = await context.Author.SingleAsync(a => a.Id == 1);
        Author? author2 = await context.Author.SingleOrDefaultAsync(a => a.Id == 1);
    }


    public async Task RetrieveSingleAuthorWithFirst()
    {
        await using BookAppDbContext context = new();
        Author author1 = await context.Author.FirstAsync(a => a.Id == 1);
        Author? author2 = await context.Author.FirstOrDefaultAsync(a => a.Id == 1);
    }


    public async Task RetrieveAll()
    {
        await using BookAppDbContext context = new();
        List<Author> list = await context.Author.ToListAsync();
    }


    public async Task RetrieveSome()
    {
        await using BookAppDbContext context = new();
        IQueryable<Book> query = context.Books.AsQueryable();
        query = query.Where(book => book.Price < 15);
        query = query.Where(book => book.Reviews.Count() > 50);
        List<Book> result = await query.ToListAsync();
    }


    public async Task LoadRelated()
    {
        await using BookAppDbContext context = new();
        Book? book = await context.Books
            .Include(book => book.PriceOffer)
            .Include(book => book.Reviews)
            .Include(book => book.AuthorLinks)
            .ThenInclude(link => link.Author)
            .FirstOrDefaultAsync(book => book.Id == 1);
    }


    public async Task AggFunctions()
    {
        await using BookAppDbContext context = new();
        decimal totalPrice = await context.Books.SumAsync(book => book.Price);

        double averageRating = await context.Reviews
            .Where(review => review.BookId == 1)
            .AverageAsync(review => review.Rating);

        decimal max = await context.Books.MaxAsync(book => book.Price);
        decimal min = await context.Books.MinAsync(book => book.Price);
        int countAll = await context.Books.CountAsync();
        int countSome = await context.Books.CountAsync(book => book.Price < 15);
        bool anyAtAll = await context.Books.AnyAsync();
        bool anyOfSome = await context.Books.AnyAsync(book => book.Price < 15);
    }

    public class AuthorNameOnly
    {
        public string AuthorName { get; set; }
    }


    public async Task LoadAuthorName()
    {
        await using BookAppDbContext context = new();
        List<AuthorNameOnly> list = await context.Author
            .Select(author => new AuthorNameOnly
            {
                AuthorName = author.Name
            })
            .ToListAsync();
    }

    public record BookTitleAndPrice(string Title, decimal Price);

    public async Task LoadBookPriceAndTitle()
    {
        await using BookAppDbContext context = new();
        List<BookTitleAndPrice> result = await context.Books
            .Select(book => new BookTitleAndPrice(book.Title, book.Price))
            .ToListAsync();
    }

    public record BookListing(
        int BookId,
        string Title,
        decimal Price,
        DateOnly PublishedOn,
        decimal ActualPrice,
        string? PromotionalText,
        string AuthorsOrdered,
        int ReviewsCount,
        double ReviewsAverageRating,
        string[] CategoryTags);

    public async Task LoadBookListings()
    {
        await using BookAppDbContext context = new();
        List<BookListing> bookListings = await context
            .Books
            .Select(book => new BookListing
            (
                book.Id,
                book.Title,
                book.Price,
                DateOnly.FromDateTime(book.PublishDate),
                book.PriceOffer == null ? book.Price : book.PriceOffer.PromotionalPrice,
                book.PriceOffer == null ? null : book.PriceOffer.PromotionalText,
                string.Join(", ", book.AuthorLinks.OrderBy(link => link.Order).Select(link => link.Author.Name)),
                book.Reviews.Count,
                book.Reviews.Average(review => review.Rating),
                book.Categories.Select(category => category.Name).ToArray()
            ))
            .ToListAsync();
    }
}