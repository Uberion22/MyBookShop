using BaseOfJob.Models;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Repository
{
    public class MSSQLBookRepository : IRepository<Book>
    {
        private readonly BookContext db;
        private bool disposed = false;

        public MSSQLBookRepository(BookContext bookContext)
        {
            db = bookContext;
        }

        public IQueryable<Book> GetBooksList()
        {
            return db.Books;
        }

        public void Create(Book item)
        {
            db.Books.Add(item);
        }

        public void Edit(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
            {
                db.Books.Remove(book);
                Save();
            }
        }

        public Book GetBookById(int id)
        {
            return db.Books.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //public void AddBooksByParser(IEnumerable<Book> item)
        //{

        //}
    }
}
