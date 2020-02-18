using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;

namespace BookTestProject.Models {
    public class BookViewModel : IValidatableObject {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [MaxLength(50, ErrorMessage = "Имя не долно быть больше 50 символов")]
        public string Name { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Index("Ix_ProductName", Order = 1, IsUnique = true)]
        [RegularExpression("[0-9-]{1,}", ErrorMessage = "ISBN некорректно заполнен")]
        public string Isbn { get; set; }

        public SelectList Authors { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {   
            BookContext db = new BookContext();
            var validateName = db.Book.FirstOrDefault(x => x.Isbn == Isbn && x.Id!=Id);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("ISBN уже существует, введите другой ISBN", new[] { "Isbn" });
                yield return errorMessage;
            } else
            {
                yield return ValidationResult.Success;
            }
        }
    }

}