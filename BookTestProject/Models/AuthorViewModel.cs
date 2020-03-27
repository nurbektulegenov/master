using System.ComponentModel.DataAnnotations;

namespace BookTestProject.Models {
    public class AuthorViewModel
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public virtual string UserName { get; set; }

    }
}