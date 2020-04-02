using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Entities.Helpers;
using BookTestProject.Models;
using NHibernate;
using NHibernate.Linq;

namespace BookTestProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBookData(int pageIndex = 1)
        {
            BookViewModel books = new BookViewModel();
            books.RowsCount = 10000;
            //books.PagesSize = GetBooksCount();
            int startIndex = (pageIndex - 1) * books.RowsCount;
            books.Books = GetBooks(startIndex, books);
            return Json(new {data = books }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBooksForSearch(string name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var books = session.Query<Books>().Select(b => b.Name.Contains(name)).ToList();
                return Json(new {data = books}, JsonRequestBehavior.AllowGet);
            }
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
            using (ISession session = NHibernateHelper.OpenSession())
            {
                long count = session.Query<TotalCounts>().Select(a => a.BooksCount).FirstOrDefault();
                return count;
            }
        }

        private List<BookViewModel> GetBooks(int startIndex, BookViewModel books)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var booksList = session.Query<Books>().Select(b => new BookViewModel()
                {
                    Id = b.Id,
                    Name = b.Name,
                    AuthorName = b.Authors.UserName,
                    Isbn = b.Isbn
                }).OrderBy(u => u.Id).Skip(startIndex).Take(books.RowsCount).ToList();
                return booksList;
            }
        }
    

        private SelectList GetAuthorsSelectList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var model = new BookViewModel();
                var authorName = session.Query<Authors>().Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.UserName
                }).ToArray();
                model.Authors = new SelectList(authorName, "Value", "Text");
                return model.Authors;
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var book = session.Get<Books>(id);
                return View(new BookViewModel
                {
                    Name = book.Name,
                    Isbn = book.Isbn,
                    Authors = GetAuthorsSelectList()
                });
            }
        }

        [HttpPost]
        public ActionResult EditBook(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var updateBook = session.Get<Books>(book.Id);
                    updateBook.Name = book.Name;
                    updateBook.Authors.UserName = book.AuthorName;
                    updateBook.Isbn = book.Isbn;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(updateBook);
                        transaction.Commit();
                    }
                }
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
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    TotalCounts count = new TotalCounts();
                    if (ModelState.IsValid)
                    {
                        Books book = new Books();
                        book.Name = bookViewModel.Name;
                        book.AuthorId = Convert.ToInt32(bookViewModel.AuthorName);
                        book.Isbn = bookViewModel.Isbn;
                        session.Save(book);
                        
                        long total = GetBooksCount();
                        total += 1;
                        count.BooksCount = total;
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }
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
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    TotalCounts count = new TotalCounts();
                    Books deleteBook = session.Get<Books>(id);
                    if (deleteBook != null)
                    {
                        session.Delete(deleteBook);
                        long total = GetBooksCount();
                        total -= 1;
                        count.BooksCount = total;
                        transaction.Commit();
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}