using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Repository
{
    public interface IRepository<T> : IDisposable
    where T : class
    {
        //получение всех книг
        IQueryable<T> GetBooksList();
        T GetBookById(int id);
        void AddBooksByParser(IEnumerable<T> item);
        //создание книги
        void Create(T item);
        //обновление(редактирование) книги
        void Edit(T item);
        //удалении вакансии
        void Delete(int id);//удалении книги
                            
        void Save();
    }

}
