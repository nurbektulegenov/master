namespace BookTestProject.Entities{
    public class Books{
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int AuthorId { get; set; }

        public virtual Authors Authors { get; set; }

        public virtual string Isbn { get; set; }
    }
}