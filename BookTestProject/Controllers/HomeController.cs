using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using BookTestProject.Models;
using BookTestProject.Repository;

namespace BookTestProject.Controllers
{
    public class HomeController : Controller
    {
        private IRepository entity;
        public HomeController()
        {
            entity = new EntityFrameworkRepository();
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
            books.PagesSize = entity.GetBooksCount();
            int startIndex = (pageIndex - 1) * books.RowsCount;
            books.Books = entity.GetBooksList(startIndex, books);
            return Json(new {data = books }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var model = new BookViewModel()
            {
                Authors = entity.GetAuthorsSelectList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = entity.GetBook(id);
            return View(new BookViewModel
            {
                Name = book.Name,
                Isbn = book.Isbn,
                Authors = entity.GetAuthorsSelectList()
            });
        }

        [HttpPost]
        public ActionResult EditBook(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                var oldBook = entity.GetBook(book.Id);
                oldBook.Name = book.Name;
                oldBook.Authors.UserName = book.AuthorName;
                oldBook.Isbn = book.Isbn;
                entity.Save();
                return RedirectToAction("Index");
            }
            var authors = new BookViewModel()
            {
                Authors = entity.GetAuthorsSelectList()
            };
            return View("Edit", authors);
        }

        [HttpPost]
        public ActionResult AddBook(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    Name = bookViewModel.Name,
                    AuthorId = Convert.ToInt32(bookViewModel.AuthorName),
                    Isbn = bookViewModel.Isbn
                };
                entity.CreateBook(book);
                ChangeBooksCount("add");
                entity.Save();
                return RedirectToAction("Index");
            }
            var bk = new BookViewModel()
            {
                Authors = entity.GetAuthorsSelectList()
            };
            return View(bk);
        }

        private long ChangeBooksCount(string mode)
        {
            TotalCount count = new TotalCount();
            long total = entity.GetBooksCount();
            if(mode=="add") total += 1;
            if(mode=="delete")total -= 1;
            return count.BooksCount = total;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            entity.DeleteBook(id);
            ChangeBooksCount("delete");
            entity.Save();
            return RedirectToAction("Index");
        }
    }
}