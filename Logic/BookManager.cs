using BookShop.Models;
using BookShop.Repository;
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
            var books =GetBooksList();
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
        }
        
        public void AddBooksFrom(IEnumerable<Book> bookList)
        {
            _repository.AddBooksByParser(bookList);
        }
        
        public void EditBook(Book book)
        {
            _repository.Edit(book);
        }
        
        public void DeleteBook(int id)
        {
            _repository.Delete(id);
        }
    }
}
