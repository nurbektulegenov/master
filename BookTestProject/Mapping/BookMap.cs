using BookTestProject.Entities;
using FluentNHibernate.Mapping;

namespace BookTestProject.Mapping
{
    public class BookMap : ClassMap<Books>
    {
        public BookMap()
        {
            Table("Books");
            Id(b => b.Id);
            Map(b => b.Name);
            Map(b => b.Isbn);
            References(b => b.Authors).Column("AuthorId");
        }
    }
}