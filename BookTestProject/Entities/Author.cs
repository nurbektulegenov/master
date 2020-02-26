using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTestProject.Entities {
    public class Author
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}