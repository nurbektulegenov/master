using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Models;

namespace BookTestProject.Controllers
{
    public class AuthorController : Controller
    {
        BookContext db = new BookContext();
        // GET
        public ActionResult Index()
        {
            var authors = db.Author.Select(a => new AuthorViewModel() {
                UserName = a.UserName
            });
            return View("Index", authors);
        }

        [HttpGet]
        public ViewResult AddAuthor() {
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                Author _author = new Author()
                {
                    UserName = author.UserName
                };
                db.Author.Add(_author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var author = db.Author.Include(b => b.Id).Select(b => new AuthorViewModel() {
                UserName = b.UserName
            }).SingleOrDefault(b => b.Id == id);
            return View(author);
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                var _author = db.Author.Find(author.Id);
                _author.UserName = author.UserName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", author);
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            Author author = db.Author.Find(id);
            if(author!= null) {
                db.Author.Remove(author);
                db.SaveChanges();
                ViewBag.Message = string.Format("Автор [{0}] удален!", id);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchBook(string name)
        {
            var books = db.Book.Where(a => a.Author.UserName.Contains(name)).ToList();
            if (books.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(books.Count);
        }
    }
}