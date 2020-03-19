namespace BookTestProject.Entities{
    public class Book{
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int AuthorId { get; set; }

        public virtual Author Authors { get; set; }

        public virtual string Isbn { get; set; }
    }
}