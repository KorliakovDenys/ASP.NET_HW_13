using ASP.NET_HW_13.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_HW_13.Data {
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
}