using System.Collections.Generic;
using BookTestProject.Interfaces;
using FluentNHibernate.Data;

namespace BookTestProject.Entities {
    public class Authors : Entity, ISoftDeletable
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual IList<Books> Books { get; set; }
        #region ISoftDeletable Members
        public virtual bool IsDeleted { get; set; }
        #endregion
        public Authors()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Books = new List<Books>();
        }
    }
}