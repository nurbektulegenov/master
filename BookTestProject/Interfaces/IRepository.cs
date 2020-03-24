using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BookTestProject.Entities;
using BookTestProject.Models;

namespace BookTestProject.Interfaces {
    interface IRepository : IDisposable
    {
        List<BookViewModel> GetBooksList(int index, BookViewModel book);
        long GetBooksCount();
        Book GetBook(int id);
        void CreateBook(Book book);
        void DeleteBook(int id);

        Author GetAuthor(int id);
        AuthorViewModel GetAuthorForEdit(int id);
        IEnumerable<AuthorViewModel> GetAuthorsList();
        SelectList GetAuthorsSelectList();
        void CreateAuthor(Author author);
        void DeleteAuthor(string authorName);
        void Save();
    }
}