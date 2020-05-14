using BookTestProject.Entities;
using FluentNHibernate.Mapping;

namespace BookTestProject.Mapping
{
    public class AuthorMap :ClassMap<Authors>
    {
        public AuthorMap()
        {
            Table("Authors");
            Where("IsDeleted = 0");
            Id(a => a.Id);
            Map(a => a.UserName);
            Map(a => a.IsDeleted);
            HasMany(a => a.Books)
                .Inverse()
                .Cascade.All();
        }
    }
}