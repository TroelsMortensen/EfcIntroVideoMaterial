using Microsoft.EntityFrameworkCore;
using VideoExamples.Entities;

namespace VideoExamples.DataAccess;

public class BookAppDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<PriceOffer> PriceOffers => Set<PriceOffer>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Author> Author => Set<Author>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source = C:\TRMO\RiderProjects\EfcIntroduction\VideoExamples\BookDatabase.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceOffer>()
            .HasIndex(offer => offer.BookId)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasKey(category => category.Name);

        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });
    }
}