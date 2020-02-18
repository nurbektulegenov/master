using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Models;

namespace BookTestProject.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();
        public ActionResult Index()
        {
            var books = db.Book.Select(b => new BookViewModel()
            {
                Id = b.Id,
                Name = b.Name,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.UserName,
                Isbn = b.Isbn
            });
            ViewBag.Book = books;
            return View("Index", books);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var model = new BookViewModel()
            {
                Authors = GetSelectList()
            };
            return View(model);
        }

        private SelectList GetSelectList()
        {
            var authorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            });
            var selctList = new SelectList(authorName, "Value", "Text");
            return selctList;
        }

        public JsonResult IsIsbnExists(string isbn)
        {
            var validate= db.Book.FirstOrDefault(x => x.Isbn == isbn);
            if (validate != null) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            IEnumerable<SelectListItem> AuthorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            });
            var book = db.Book.Single(b => b.Id == id);
            ViewData["AuthorId"] = AuthorName;
            return View(new BookViewModel {
                Id = book.Id,
                Name = book.Name,
                AuthorId = book.AuthorId,
                Isbn = book.Isbn
            });
        }

        [HttpPost]
        public ActionResult EditBook(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                var _bk = db.Book.Find(book.Id);
                _bk.Name = book.Name;
                _bk.AuthorId = book.AuthorId;
                _bk.Isbn = book.Isbn;
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                var bk = new BookViewModel()
                {
                    Authors = GetSelectList()
                };
                return View("Edit", bk);
            }
        }

        [HttpPost]
        public ActionResult AddBook(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                Book _book = new Book()
                {
                    Name = book.Name,
                    AuthorId = Convert.ToInt32(book.AuthorName),
                    Isbn = book.Isbn
                };
                db.Book.Add(_book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var bk = new BookViewModel()
                {
                    Authors = GetSelectList()
                };
                return View(bk);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Book book = db.Book.Find(id);
            if (book != null)
            {
                db.Book.Remove(book);
                db.SaveChanges();
                ViewBag.Message = string.Format("Книга № [{0}] удалена!", id);
            }
            return RedirectToAction("Index");
        }
    }
}