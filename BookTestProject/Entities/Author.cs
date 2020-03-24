using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTestProject.Entities {
    public class Author
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}