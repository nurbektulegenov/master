using System.Data.Entity;
using System.Text;

namespace BookTestProject.Entities {
    public class BookContext : DbContext{
        public BookContext() : base("DbConnection") { }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasIndex(u => u.Isbn).IsUnique();
        }
    }
}