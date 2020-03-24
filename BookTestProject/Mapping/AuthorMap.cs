using BookTestProject.Entities;
using FluentNHibernate.Mapping;

namespace BookTestProject.Mapping
{
    public class AuthorMap :ClassMap<Author>
    {
        public AuthorMap()
        {
            Table("Authors");
            Id(a => a.Id);
            Map(a => a.UserName);
            HasMany(a => a.Books)
                .Inverse().
                Cascade.All();
        }
    }
}