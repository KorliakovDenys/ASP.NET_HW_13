using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class DataContextInitializer : IDbInitializer {
    private readonly DataContext _context;

    public DataContextInitializer(DataContext context) {
        _context = context;
    }

    public void Initialize() {
        // Apply any pending migrations
        _context.Database.Migrate();

        // Seed initial authors if they don't exist
        if (!_context.Authors!.Any()) {
            var author1 = new Author { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1980, 1, 1) };
            var author2 = new Author
                { FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1990, 5, 15) };

            _context.Authors.AddRange(author1, author2);
            _context.SaveChanges();
        }

        // Seed 15 books
        if (!_context.Books.Any()) {
            for (var i = 1; i <= 15; i++) {
                var authorId = i % 2 == 0 ? 1 : 2; // Alternate between authors
                var book = new Book {
                    Title = $"Book {i}",
                    AuthorId = authorId,
                    PublicationDate = DateTime.Now.AddMonths(-i) // Adjust the publication dates as needed
                };

                _context.Books.Add(book);
            }

            _context.SaveChanges();
        }
    }
}