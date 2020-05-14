using BookTestProject.Entities;
using FluentNHibernate.Mapping;

namespace BookTestProject.Mapping
{
    public class BookMap : ClassMap<Books>
    {
        public BookMap()
        {
            Table("Books");
            Where("DELETED = 0");
            Id(b => b.Id);
            Map(b => b.Name);
            Map(b => b.Isbn);
            Map(b => b.IsDeleted);
            References(b => b.Authors).Column("AuthorId");
        }
    }
}