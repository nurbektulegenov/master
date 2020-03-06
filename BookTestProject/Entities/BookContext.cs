using System.Data.Entity;

namespace BookTestProject.Entities {
    public class BookContext : DbContext{
        public BookContext() : base("DbConnection") { }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<TotalCount> TotalCount { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithRequired(a => a.Authors)
                .HasForeignKey(a=>a.AuthorId);
            modelBuilder.Entity<Book>().HasIndex(u => u.Isbn).IsUnique();
        }
    }
}