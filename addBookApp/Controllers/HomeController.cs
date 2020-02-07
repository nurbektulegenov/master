using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using addBookApp.Models;

namespace addBookApp.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Save(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
        }

        public ActionResult BooksPartialView()
        {
            
            IEnumerable<Book> books = db.Books.ToArray();
            return PartialView("BooksPartialView",books);
        }
    }
}