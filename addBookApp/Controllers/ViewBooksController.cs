using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using addBookApp.Models;

namespace addBookApp.Controllers
{
    public class ViewBooksController : Controller
    {
        BookContext db = new BookContext();

        public ActionResult ViewBooks()
        {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View("View", books);
        }
    }
}