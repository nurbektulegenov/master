using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTestProject.Entities {
    public class Author
    {
        public virtual int Id { get; private set; }
        public virtual string UserName { get; set; }
        public virtual ISet<Book> Books { get; private set; }
        public Author() {
            Books=new HashSet<Book>();
        }
    }
    
}