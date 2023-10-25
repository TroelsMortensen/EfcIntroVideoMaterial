// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoExamples.DataAccess;
using VideoExamples.Entities;
using VideoExamples.Migrations;

// Book book = new()
// {
//     Id = 1,
//     Price = 50.50m,
//     Publisher = "VIA bookstore",
//     Title = "SomeBook",
//     PublishDate = DateTime.Parse("2023-10-24 15:27:31.1867944")
// };
//
// Book resultingBook = await InsertBookAsync(book);

// await UpdateBook(book);

// Book? retrieved = await GetBook(2);
// retrieved.Price = 42.00m;
//
// await UpdateBook(retrieved);

// await DeleteBook(2);

// PriceOffer po = new()
// {
//     PromotionalPrice = 15m,
//     PromotionalText = "super cheap",
//     BookId = 1
// };
//
// await AddPriceOffer(po);

// PriceOffer po = new()
// {
//     PromotionalPrice = 37m,
//     PromotionalText = "Not quite so super cheap"
// };
//
// await AddPriceOfferToBook(po, 2);

// Book bookWithPriceOffer = await GetBookWithPriceOffer(1);

Review review = new()
{
    Comment = "Decent BOok",
    Rating = 4,
    VoterName = "Troels",
};

// await AddReviewToBookUsingFk(review);

// await AddReviewToBook(review, 1);

// await RemoveReviewFromBook(1, 2);

// Book both = await LoadWithBoth(1);
// int stop = 0;



await AddCategoryToBook("Sci-Fi", 1);

async Task AddCategoryToBook(string catId, int bookId)
{
    using BookAppDbContext context = new();
    Book? existingBook = await context.Books
        .Include(book => book.Categories)
        .FirstOrDefaultAsync(book => book.Id == bookId);

    Category? existingCategory = await context.Categories
        .Include(cat => cat.Books)
        .FirstOrDefaultAsync(cat => cat.Name.Equals(catId));
    
    existingCategory.Books.Add(existingBook);

    context.Categories.Update(existingCategory);
    await context.SaveChangesAsync();
}

async Task<Book> LoadWithBoth(int bookId)
{
    using BookAppDbContext context = new();
    Book? book = await context.Books
        .Include(book => book.PriceOffer)
        .Include(book => book.Reviews)
        .FirstOrDefaultAsync(book => book.Id == bookId);
    return book;
}


async Task RemoveReviewFromBook(int bookId, int reviewId)
{
    using BookAppDbContext context = new();
    Review? toBeRemoved = await context.Reviews.FindAsync(reviewId);
    context.Reviews.Remove(toBeRemoved);
    await context.SaveChangesAsync();
}

// not working:
async Task AddReviewToBook(Review review, int bookId)
{
    using BookAppDbContext context = new();
    Book? foundBook = await context.Books
        .Include(book => book.Reviews)
        .FirstOrDefaultAsync(book => book.Id == bookId);
    foundBook.Reviews.Add(review);

    context.Books.Update(foundBook);
    await context.SaveChangesAsync();
}

async Task AddReviewToBookUsingFk(Review review)
{
    using BookAppDbContext context = new();
    await context.Reviews.AddAsync(review);
    await context.SaveChangesAsync();
}

async Task<Book> GetBookWithPriceOffer(int id)
{
    using BookAppDbContext ctx = new();
    Book? foundBook = await ctx.Books
        .Include(book => book.PriceOffer)
        .FirstOrDefaultAsync(book => book.Id == id);

    return foundBook!;
}

async Task AddPriceOfferToBook(PriceOffer po, int bookId)
{
    using BookAppDbContext context = new();
    Book foundBook = (await context.Books.FindAsync(bookId))!;
    foundBook.PriceOffer = po;
    context.Books.Update(foundBook);
    await context.SaveChangesAsync();
}


async Task AddPriceOffer(PriceOffer po)
{
    using BookAppDbContext context = new();
    await context.PriceOffers.AddAsync(po);
    await context.SaveChangesAsync();
}

async Task DeleteBook(int id)
{
    using BookAppDbContext context = new();
    Book? bookToDelete = await context.Books.FindAsync(id);
    context.Books.Remove(bookToDelete);
    await context.SaveChangesAsync();
}

async Task<Book?> GetBook(int id)
{
    using BookAppDbContext context = new();
    Book? foundBook = await context.Books.FindAsync(id);
    return foundBook;
}

async Task UpdateBook(Book book)
{
    using BookAppDbContext context = new();
    context.Books.Update(book);
    await context.SaveChangesAsync();
}

async Task<Book> InsertBookAsync(Book book)
{
    using BookAppDbContext context = new();
    EntityEntry<Book> entry = await context.Books.AddAsync(book);
    await context.SaveChangesAsync();
    return entry.Entity;
}