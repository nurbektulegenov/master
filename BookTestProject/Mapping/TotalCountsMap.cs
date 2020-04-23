using BookTestProject.Entities;
using FluentNHibernate.Mapping;

namespace BookTestProject.Mapping
{
    public class TotalCountsMap : ClassMap<TotalCounts>
    {
        public TotalCountsMap()
        {
            Table("TotalCounts");
            Id(b => b.Id);
            Map(b => b.BooksCount);
    }
    }
}