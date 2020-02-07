using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.BookId = id;
            return View();
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            db.Books.AddOrUpdate(book);
            db.SaveChanges();
            return RedirectToAction("ViewBooks");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("ViewBooks");
        }
    }
}