using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using addBookApp.Models;

namespace addBookApp.Controllers {
    public class ViewBooksController : Controller {
        BookContext db = new BookContext();
        public ActionResult ViewBooks() {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View("View", books);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            Book book = db.Books.Find(id);
            return View(book);
        }

        [HttpPost]
        public ActionResult EditBook(Book book) {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewBooks");
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            Book book = db.Books.Find(id);
            if(book!= null) {
                db.Books.Remove(book);
                db.SaveChanges();
            }
            return RedirectToAction("ViewBooks");
        }
    }
}