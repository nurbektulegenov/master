using System.ComponentModel.DataAnnotations;

namespace addBookApp.Models{
    public class Book{
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Заполните поел")]
        public string Isbn { get; set; }
    }
}