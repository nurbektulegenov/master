using BookTestProject.Interfaces;
using FluentNHibernate.Data;

namespace BookTestProject.Entities{
    public class Books : Entity, ISoftDeletable {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual Authors Authors { get; set; }

        public virtual string Isbn { get; set; }
        public virtual bool IsDeleted { get; set; }

        public Books()
        {
            Authors = new Authors();
        }
    }
}