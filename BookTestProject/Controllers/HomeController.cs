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
            return View();
        }

        [HttpPost]
        public ActionResult GetBookData(int pageIndex = 1)
        {
            BookViewModel books = new BookViewModel();
            books.RowsCount = 10000;
            books.PagesSize = GetBooksCount();
            int startIndex = (pageIndex - 1) * books.RowsCount;
            books.Books = GetBooks(startIndex, books);
            return Json(new {data = books }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBooksForSearch(string name)
        {
            var books = db.Book.Select(b => b.Name.Contains(name)).ToList();
            return Json(new { data = books }, JsonRequestBehavior.AllowGet);
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
        private List<BookViewModel> GetBooks(int startIndex, BookViewModel books) {
            var booksList = db.Book.Select(b => new BookViewModel() {
                Id = b.Id,
                Name = b.Name,
                AuthorName = b.Authors.UserName,
                Isbn = b.Isbn
            }).OrderBy(u => u.Id).Skip(startIndex).Take(books.RowsCount).ToList();
            return booksList;
        }

        private SelectList GetAuthorsSelectList()
        {
            var authorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            }).ToArray();
            var selectList = new SelectList(authorName, "Value", "Text");
            return selectList;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = db.Book.Find(id);
            return View(new BookViewModel
            {
                Name = book.Name,
                Isbn = book.Isbn,
                Authors = GetAuthorsSelectList()
            });
        }

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
        public ActionResult AddBook(BookViewModel bookViewModel)
        {
            TotalCount count = new TotalCount();
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    Name = bookViewModel.Name,
                    AuthorId = Convert.ToInt32(bookViewModel.AuthorName),
                    Isbn = bookViewModel.Isbn
                };
                db.Book.Add(book);
                long total = db.TotalCount.Select(a => a.BooksCount).First();
                total += 1;
                count.BooksCount = total;
                db.SaveChanges();
                return RedirectToAction("Index");
                /*return RedirectToRoutePermanent(new {
                    Controller = "Home",
                    Action = "Index"
                });*/
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
                long total = db.TotalCount.Select(a => a.BooksCount).First();
                total -= 1;
                count.BooksCount = total;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}