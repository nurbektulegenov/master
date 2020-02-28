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
            }).ToArray();
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

        [HttpGet]
        public JsonResult BooksToAuthors(string name)
        {
            var count = db.Book.Where(a => a.Authors.UserName == name).ToList().Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string UserName)
        {
            Author author = db.Author.FirstOrDefault(a=>a.UserName == UserName);
            if (author != null)
            {
                db.Author.Remove(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Json(HttpNotFound());
        }
    }
}