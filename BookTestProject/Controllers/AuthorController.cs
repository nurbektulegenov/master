using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Helpers;
using BookTestProject.Models;
using NHibernate;

namespace BookTestProject.Controllers
{
    public class AuthorController : Controller
    {
        // GET
        public ActionResult Index()
        {
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                var authors = session.Query<Author>().Select(a => new AuthorViewModel()
                {
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
                using (ISession session = FluentNHibernateHelper.OpenSession())
                {
                    Author author = new Author();
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
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                var author = session.Query<Author>().Select(b => new AuthorViewModel() {
                    UserName = b.UserName
                }).SingleOrDefault(b => b.Id == id);
                return View(author);
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = FluentNHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var author = session.Get<Author>(authorViewModel.Id);
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
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                var count = session.Query<Book>().Where(a => a.Authors.UserName == name).ToList().Count;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(string userName)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                Author author = session.Query<Author>().FirstOrDefault(a => a.UserName == userName);
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