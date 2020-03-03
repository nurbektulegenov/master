using System.Collections.Generic;

namespace BookTestProject.Entities{
    public class Book{
        public int Id { get; set; }

        public string Name { get; set; }

        public int AuthorId { get; set; }

        public Author Authors { get; set; }

        public string Isbn { get; set; }
    }
}