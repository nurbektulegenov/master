using System;
using System.Collections.Generic;
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
            ViewBag.BooksCount = GetBooksCount();
            return View();
        }

        [HttpPost]
        public ActionResult GetBookData(int pageIndex=1)
        {
            BookViewModel _books = new BookViewModel();
            _books.RowsCount = 2000;
            _books.PagesSize = GetBooksCount();
            int startIndex = (pageIndex - 1) * _books.RowsCount;
            _books.Books = GetBooks(startIndex, _books);
            return Json(new {data = _books }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var model = new BookViewModel()
            {
                Authors = GetAuthorsSelectList()
            };
            return View(model);
        }

        private long GetBooksCount()
        {
            return db.TotalCount.Select(a => a.BooksCount).First();
        }
        private List<BookViewModel> GetBooks(int startIndex, BookViewModel _books) {
            var books = db.Book.Select(b => new BookViewModel() {
                Id = b.Id,
                Name = b.Name,
                AuthorName = b.Authors.UserName,
                Isbn = b.Isbn
            }).OrderBy(u => u.Id).Skip(startIndex).Take(_books.RowsCount).ToList();
            return books;
        }

        private SelectList GetAuthorsSelectList()
        {
            var authorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            }).ToArray();
            var selctList = new SelectList(authorName, "Value", "Text");
            return selctList;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = db.Book.Single(b => b.Id == id);
            return View(new BookViewModel
            {
                Name = book.Name,
                Isbn = book.Isbn,
                Authors = GetAuthorsSelectList()
            });
        }

        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var book = db.Book.Single(b => b.Id == id);
        //    var books = new BookViewModel()
        //    {
        //        Name = book.Name,
        //        Authors = GetAuthorsSelectList(),
        //        Isbn = book.Isbn
        //    };
        //    return PartialView("PartialViews/EditBook", books);
        //}

        [HttpPost]
        public ActionResult EditBook(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                var bk = db.Book.Find(book.Id);
                bk.Name = book.Name;
                bk.Authors.UserName = book.AuthorName;
                bk.Isbn = book.Isbn;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var authors = new BookViewModel()
            {
                Authors = GetAuthorsSelectList()
            };
            return View("Edit", authors);
        }

        [HttpPost]
        public ActionResult AddBook(BookViewModel book)
        {
            TotalCount count = new TotalCount();
            if (ModelState.IsValid)
            {
                Book _book = new Book()
                {
                    Name = book.Name,
                    AuthorId = Convert.ToInt32(book.AuthorName),
                    Isbn = book.Isbn
                };
                db.Book.Add(_book);
                long total = Convert.ToInt64(db.TotalCount.Select(a=>a.BooksCount));
                total += 1;
                count.BooksCount = total;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var bk = new BookViewModel()
            {
                Authors = GetAuthorsSelectList()
            };
            return View(bk);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TotalCount count = new TotalCount();
            Book book = db.Book.Find(id);
            if (book != null)
            {
                db.Book.Remove(book);
                long total = Convert.ToInt64(db.TotalCount.Select(a => a.BooksCount));
                total -= 1;
                count.BooksCount = total;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}