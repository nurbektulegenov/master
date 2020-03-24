using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using BookTestProject.Models;

namespace BookTestProject.Repository
{
    public class EntityFrameworkRepository : IRepository
    {
        private BookContext db;

        public EntityFrameworkRepository()
        {
            this.db = new BookContext();
        }

        public List<BookViewModel> GetBooksList(int startIndex, BookViewModel books)
        {
            var booksList = db.Book.Select(b => new BookViewModel() {
                Id = b.Id,
                Name = b.Name,
                AuthorName = b.Authors.UserName,
                Isbn = b.Isbn
            }).OrderBy(u => u.Id).Skip(startIndex).Take(books.RowsCount).ToList();
            return booksList;
        }

        public SelectList GetAuthorsSelectList()
        {
            var authorName = db.Author.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.UserName
            }).ToArray();
            var selectList = new SelectList(authorName, "Value", "Text");
            return selectList;
        }

        public IEnumerable<AuthorViewModel> GetAuthorsList()
        {
            var author = db.Author.Select(a => new AuthorViewModel() {
                UserName = a.UserName
            }).ToArray();
            return author;
        }

        public AuthorViewModel GetAuthorForEdit(int id)
        {
            var author = db.Author.Include(b => b.Id).Select(b => new AuthorViewModel() {
                UserName = b.UserName
            }).SingleOrDefault(b => b.Id == id);
            return author;
        }

        public Book GetBook(int id)
        {
            return db.Book.Find(id);
        }

        public Author GetAuthor(int id)
        {
            return db.Author.Find(id);
        }
        
        public long GetBooksCount()
        {
            return db.TotalCount.Select(a => a.BooksCount).First();
        }
 
        public void CreateBook(Book book)
        {
            db.Book.Add(book);
        }
        public void CreateAuthor(Author author)
        {
            db.Author.Add(author);
        }
 
        public void DeleteBook(int id)
        {
            Book book = GetBook(id);
            if(book != null)
                db.Book.Remove(book);
        }

        public void DeleteAuthor(string authorName)
        {
            Author author = db.Author.FirstOrDefault(a => a.UserName == authorName);
            if (author != null)
                db.Author.Remove(author);
        }

        public void Save()
        {
            db.SaveChanges();
        }
 
        private bool disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}