using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Entities.Helpers;
using NHibernate;

namespace BookTestProject.Models {
    public class BookViewModel : IValidatableObject {
        
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [MaxLength(50, ErrorMessage = "Имя не долно быть больше 50 символов")]
        public virtual string Name { get; set; }

        public virtual string AuthorName { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Index("Ix_ProductName", Order = 1, IsUnique = true)]
        [RegularExpression("[0-9-]{1,}", ErrorMessage = "ISBN некорректно заполнен")]
        public virtual string Isbn { get; set; }

        public long PagesSize { get; set; }
        public int RowsCount { get; set; }

        public virtual IList<BookViewModel> Books { get; set; }

        public virtual SelectList Authors { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var validateName = session.Query<Books>().FirstOrDefault(x => x.Isbn == Isbn && x.Id != Id);
                Console.WriteLine(validateName);
                if (validateName != null)
                {
                    ValidationResult errorMessage =
                        new ValidationResult("ISBN уже существует, введите другой ISBN", new[] {"Isbn"});
                    yield return errorMessage;
                }
                else
                {
                    yield return ValidationResult.Success;
                }
            }
        }
        //public BookViewModel()
        //{
        //    Authors = new SelectList(Authors, "Value", "Text");
        //}
    }
}
