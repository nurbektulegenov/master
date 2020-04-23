using BookTestProject.Entities;
using BookTestProject.Interfaces;
using BookTestProject.Models;
using BookTestProject.Repository;
using System.Web.Mvc;

namespace BookTestProject.Controllers
{
    public class AuthorController : Controller
    {
        IGenericRepository<Authors> authorRep = new GenericRepository<Authors>(null);

        public ActionResult Index()
        {
            var authors = authorRep.Select(a => new AuthorViewModel()
            {
                Id = a.Id,
                UserName = a.UserName
            }).ToArray();
            return View("Index", authors);
        }

        [HttpGet]
        public ViewResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                Authors author = new Authors();
                author.UserName = authorViewModel.UserName;
                authorRep.Add(author);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var author = authorRep.GetById(id);
            return View(new AuthorViewModel
            {
                Id = author.Id,
                UserName = author.UserName
            });
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                var author = authorRep.GetById(authorViewModel.Id);
                author.UserName = authorViewModel.UserName;
                authorRep.Update(author);
                return RedirectToAction("Index");
            }
            return View("Edit", authorViewModel);
        }

        [HttpGet]
        public JsonResult BooksToAuthors(string name)
        {
            var count = authorRep.Where(a => a.UserName == name).Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string userName)
        {
            Authors author = authorRep.Find(a => a.UserName == userName);
            if (author != null)
            {
                authorRep.Delete(author.Id);
                return RedirectToAction("Index");
            }

            return Json(HttpNotFound());
        }
    }
}