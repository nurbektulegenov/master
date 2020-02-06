using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace addBookApp.Models
{
    public class BookContext : DbContext
    {
        public BookContext() : base("DbConnection") { }

        public DbSet<Book> Books { get; set; }
    }
}