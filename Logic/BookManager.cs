using BookShop.Models;
using BookShop.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.parser
{
    public class BookManager
    {
        private IRepository<Book> _repository;

        public BookManager(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public IQueryable<Book> GetBooksList()
        {
            return _repository.GetBooksList();
        }

        public Book GetBookById(int id)
        {
            return _repository.GetBookById(id);
        }

        /// <summary>
        /// Метод возвращает список книг, удовлетворяющих устовиям поиска хранящихся в соответствующийх полях объекта класса FilterOptions  
        /// </summary>
        /// <param name="filter">экземпляр класса содержащий параметры фильрации</param>
        /// <returns></returns>
        public IQueryable<Book> GetBooksWithFilter(FilterOptions filter)
        {
            var books = GetBooksList();
            if (filter.NameOfBook != null)
                books = books.Where(p => p.Name.Contains(filter.NameOfBook));
            if (filter.Price > 0)
                books = books.Where(p => (p.Price <= filter.Price)); ;
            if (filter.Author != null)
                books = books.Where(p => p.Author.Contains(filter.Author));
            if (filter.Genere != null)
                books = books.Where(p => p.Genre.Contains(filter.Genere));

            return books;
        }

        public void AddBook(Book book)
        {
            _repository.Create(book);
            _repository.Save();
        }

        public void AddBooksFromSite(IEnumerable<Book> item)
        {
            foreach (Book p in item)
            {
                var book = _repository.GetBooksList().FirstOrDefault(book => book.Name == p.Name);

                if (book != null)
                {
                    _repository.Edit(book);
                    int i = book.Id;
                    book.Name = p.Name;
                    book.ImageURL = p.ImageURL;
                    book.Author = p.Author;
                    book.Price = p.Price;
                    book.OriginalPageURL = p.OriginalPageURL;
                }

                else
                {
                    _repository.Create(p);
                }
                _repository.Save();
            }
        }

        public void EditBook(Book book)
        {
            _repository.Edit(book);
            _repository.Save();
        }

        public void DeleteBook(int id)
        {
            _repository.Delete(id);
        }
        
        
    }
}
