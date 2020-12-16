using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Repository
{
    public interface IRepository<T> : IDisposable
    where T : class
    {
        IQueryable<T> GetBooksList();

        T GetBookById(int id);

        void AddBooksByParser(IEnumerable<T> item);

        void Create(T item);

        void Edit(T item);

        void Delete(int id);       
        
        void Save();
    }

}
