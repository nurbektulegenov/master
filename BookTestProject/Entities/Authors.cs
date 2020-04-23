using System.Collections.Generic;

namespace BookTestProject.Entities {
    public class Authors
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual IList<Books> Books { get; set; }
        public Authors()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Books = new List<Books>();
        }
    }
}