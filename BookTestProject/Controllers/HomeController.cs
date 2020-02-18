using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Models;

namespace BookTestProject.Controllers {
    public class HomeController : Controller {
        BookContext db = new BookContext();
        public ActionResult Index() {
            var books = db.Book.Select(b => new BookViewModel() {
                Id = b.Id,
                Name = b.Name,
                AuthorName = b.Author.UserName,
                Isbn = b.Isbn
            });
            ViewBag.Book = books;
            return View("Index", books);
        }

        public ActionResult Add() {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var book = db.Book.Include(b => b.Author).Select(b => new BookViewModel() {
                Id=b.Id,
                Name = b.Name,
                AuthorName = b.Author.UserName,
                Isbn = b.Isbn
            }).SingleOrDefault(b => b.Id == id);
            return View(book);
        }

        [HttpPost]
        public ActionResult EditBook(BookViewModel book) {
            if (ModelState.IsValid) {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("Edit", book);
            }
        }

        [HttpPost]
        public ActionResult Add(BookViewModel book) {
            if (ModelState.IsValid) {
                db.Entry(book).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            Book book = db.Book.Find(id);
            if(book!= null) {
                db.Book.Remove(book);
                db.SaveChanges();
                ViewBag.Message = string.Format("Книга № [{0}] удалена!", id);
            }
            return RedirectToAction("Index");
        }

        //public ViewResult Authors() {
        //    return View();
        //}

        public ViewResult AddAuthor(AuthorViewModel author) {
            if (ModelState.IsValid) {
                db.Entry(author).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //public ViewResult EditAuthor() {
        //}
    }
}