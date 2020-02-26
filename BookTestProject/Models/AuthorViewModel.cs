using System.ComponentModel.DataAnnotations;

namespace BookTestProject.Models {
    public class AuthorViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public string UserName { get; set; }

    }
}