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

        public IQueryable<Book> GetBooksWithFilter()
        {
            // тут определим логику поиска
            var books =GetBooksList();

            if (FilterOptions.NameofBook != null)  // 
                books = books.Where(p => p.Name.Contains(FilterOptions.NameofBook));
            
            
            if (FilterOptions.Price > 0) 
                books = books.Where(p => (p.Price <= FilterOptions.Price)); ;
            // 
            if (FilterOptions.Author != null)
                books = books.Where(p => p.Author.Contains(FilterOptions.Author));
            // 
            if (FilterOptions.Genere != null)
                books = books.Where(p => p.Genre.Contains(FilterOptions.Genere));
            // возвращаем результат поиска
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
