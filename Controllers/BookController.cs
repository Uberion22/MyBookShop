using BookShop.Models;
using BookShop.parser;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class BookController : Controller
    {
        private readonly BookManager _manager;
        
        public BookController(BookManager manager)
        {
            _manager = manager;
        }
        
        public IActionResult Index()
        {
            ViewBag.Filter = new FilterOptions() { Price = 0};
            
            return View(_manager.GetBooksList());
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetAction()
        {
            ViewBag.Filter = new FilterOptions() { Price = 0 };
            _manager.AddBooksFrom(Parser.GetBooks(5));
            
            return View("Index", _manager.GetBooksList());
        }

        public IActionResult AddBook()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _manager.AddBook(book);
                
                return RedirectToAction("Index");
            }
            
            return View(book);
        }

        [HttpPost, ActionName("GetBookById")]
        public ActionResult DeleteConfirmed(int id)
        {
            _manager.DeleteBook(id);
            
            return RedirectToAction("Index");
        }

        public ActionResult GetBookById(int id)
        {
            var book = _manager.GetBookById(id);
            
            return View(book);
        }
        [HttpGet]
        public ActionResult SearchBooks(string name, string author, decimal price)
        {
            ViewBag.Filter = new FilterOptions() { NameOfBook = name, Author = author, Price = price };
            var bookList = _manager.GetBooksWithFilter(ViewBag.Filter);
            
            return View("Index", bookList);
        }

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            var book = _manager.GetBookById(id);
            
            return View(book);
        }
        
        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _manager.EditBook(book);
                
                return RedirectToAction("Index");
            }
            
            return View(book);
        }
    }
}
