using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class DataContext : DbContext {
    public DbSet<Book>? Books { get; set; }

    public DbSet<Author>? Authors { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict); // You can adjust the delete behavior as needed
    }
}