using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Entities.Helpers;
using BookTestProject.Models;
using NHibernate;

namespace BookTestProject.Controllers
{
    public class AuthorController : Controller
    {
        // GET
        public ActionResult Index()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var authors = session.Query<Authors>().Select(a => new AuthorViewModel()
                {
                    Id=a.Id,
                    UserName = a.UserName
                }).ToArray();
                return View("Index", authors);
            }
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
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    Authors author = new Authors();
                    author.UserName = authorViewModel.UserName;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(author);
                        transaction.Commit();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var author = session.Get<Authors>(id);
                return View(new AuthorViewModel
                {
                    Id=author.Id,
                    UserName = author.UserName
                });
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var author = session.Get<Authors>(authorViewModel.Id);
                        author.UserName = authorViewModel.UserName;
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Edit", authorViewModel);
        }

        [HttpGet]
        public JsonResult BooksToAuthors(string name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var count = session.Query<Books>().Where(a => a.Authors.UserName == name).ToList().Count;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(string userName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Authors author = session.Query<Authors>().FirstOrDefault(a => a.UserName == userName);
                if (author != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(author);
                        transaction.Commit();
                    }
                    return RedirectToAction("Index");
                }
            }
            return Json(HttpNotFound());
        }
    }
}