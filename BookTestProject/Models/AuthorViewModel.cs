using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookTestProject.Models {
    public class AuthorViewModel {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public string UserName { get; set; }
    }
}