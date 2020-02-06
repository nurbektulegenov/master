using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using addBookApp.Models;

namespace addBookApp.Controllers
{
    public class SaveController : Controller
    {
        private BookContext db = new BookContext();

        [HttpPost]
        public void Save(Book book) {
            db.Books.Add(book);
            db.SaveChanges();
        }
    }
}