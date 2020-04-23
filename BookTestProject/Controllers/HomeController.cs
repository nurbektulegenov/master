using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using BookTestProject.Models;
using BookTestProject.Repository;
using NHibernate;

namespace BookTestProject.Controllers
{
    public class HomeController : Controller
    {
        private IGenericRepository<Books> bookRepository;
        private IGenericRepository<Authors> authorRepository;
        public HomeController()
        {
            bookRepository=new GenericRepository<Books>(null);
            authorRepository=new GenericRepository<Authors>(null);
        }
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
            var books = bookRepository.Select(b => b.Name.Contains(name)).ToList();
            return Json(new {data = books}, JsonRequestBehavior.AllowGet);
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

        private List<BookViewModel> GetBooks(int startIndex, BookViewModel books)
        {
            var booksList = bookRepository.SelectBooks(b => new BookViewModel()
            {
                Id = b.Id,
                Name = b.Name,
                AuthorName = b.Authors.UserName,
                Isbn = b.Isbn
            }, u=>u.Id, startIndex, books.RowsCount);
            return booksList;
        }


        private SelectList GetAuthorsSelectList()
        {
            var model = new BookViewModel();
            var authorName = authorRepository.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            }).ToArray();
            model.Authors = new SelectList(authorName, "Value", "Text");
            return model.Authors;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = bookRepository.GetById(id);
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
                var updateBook = bookRepository.GetById(book.Id);
                updateBook.Name = book.Name;
                updateBook.Authors.UserName = book.AuthorName;
                updateBook.Isbn = book.Isbn;
                bookRepository.Add(updateBook);
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
            int value = Convert.ToInt32(bookViewModel.AuthorName);
            TotalCounts count = new TotalCounts();
            if (ModelState.IsValid)
            {
                Books book = new Books();
                book.Name = bookViewModel.Name;
                book.Authors.Id = authorRepository.GetById(value).Id;
                book.Isbn = bookViewModel.Isbn;
                bookRepository.Add(book);

                long total = bookRepository.GetBooksCount();
                using (ITransaction transaction = UnitOfWork.OpenSession().BeginTransaction())
                {
                    total += 1;
                    count.BooksCount = total;
                    transaction.Commit();
                }
                return RedirectToAction("Index");
            }
            var model = new BookViewModel()
            {
                Authors = GetAuthorsSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TotalCounts count = new TotalCounts();
            bookRepository.Delete(id);
            long total = bookRepository.GetBooksCount();
            total -= 1;
            count.BooksCount = total;
            return RedirectToAction("Index");
        }
    }
}