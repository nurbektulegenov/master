using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using BookTestProject.Models;
using BookTestProject.Repository;

namespace BookTestProject.Controllers
{
    public class AuthorController : Controller
    {
        private IRepository entity;
        public AuthorController()
        {
            entity = new EntityFrameworkRepository();
        }
        // GET
        public ActionResult Index()
        {
            var authors = entity.GetAuthorsList();
            return View("Index", authors);
        }

        [HttpGet]
        public ViewResult AddAuthor() {
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                Author author = new Author()
                {
                    UserName = authorViewModel.UserName
                };
                entity.CreateAuthor(author);
                entity.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var author = entity.GetAuthorForEdit(id);
            return View(author);
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                var author = entity.GetAuthor(authorViewModel.Id);
                author.UserName = authorViewModel.UserName;
                entity.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", authorViewModel);
        }

        /*[HttpGet]
        public JsonResult BooksToAuthors(string name)
        {
            var count = db.Book.Where(a => a.Authors.UserName == name).ToList().Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }*/

        [HttpPost]
        public ActionResult Delete(string userName)
        {
            entity.DeleteAuthor(userName);
            entity.Save();
            return RedirectToAction("Index");
        }
    }
}