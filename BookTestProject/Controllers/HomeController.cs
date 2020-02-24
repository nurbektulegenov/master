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
                Name = b.Name,
                Authors = GetAuthorsSelectList(),
                AuthorName = b.Author.UserName,
                Isbn = b.Isbn
            });
            return View("Index", books);
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

        private SelectList GetAuthorsSelectList()
        {
            var authorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            });
            var selctList = new SelectList(authorName, "Value", "Text");
            return selctList;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = db.Book.Single(b => b.Id == id);
            return View(new BookViewModel {
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
                bk.Author.UserName = book.AuthorName;
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
            if (ModelState.IsValid)
            {
                Book _book = new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    Isbn = book.Isbn
                };
                db.Book.Add(_book);
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