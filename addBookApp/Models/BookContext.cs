using System.Data.Entity;

namespace addBookApp.Models{
    public class BookContext : DbContext{
        public BookContext() : base("DbConnection") { }
        public DbSet<Book> Books { get; set; }
    }
}