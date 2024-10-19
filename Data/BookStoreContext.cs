using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext: DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookAuditLog> AuditLogs { get; set; }
    
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuditLog>()
            .HasKey(a => new { a.Timestamp, a.Isbn });
    }
}