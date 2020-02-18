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
                Id = a.Id,
                UserName = a.UserName
            });
            ViewBag.Author = authors;
            return View("Index", authors);
        }

        [HttpGet]
        public ViewResult AddAuthor() {
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(AuthorViewModel author) {
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
            else {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var author = db.Author.Include(b => b.Id).Select(b => new AuthorViewModel() {
                Id=b.Id,
                UserName = b.UserName
            }).SingleOrDefault(b => b.Id == id);
            return View(author);
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel author) {
            if (ModelState.IsValid) {
                Author _author = new Author() {
                    Id=author.Id,
                    UserName = author.UserName
                };
                db.Entry(_author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("Edit", author);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            Author author = db.Author.Find(id);
            if(author!= null) {
                db.Author.Remove(author);
                db.SaveChanges();
                ViewBag.Message = string.Format("Книга № [{0}] удалена!", id);
            }
            return RedirectToAction("Index");
        }
    }
}