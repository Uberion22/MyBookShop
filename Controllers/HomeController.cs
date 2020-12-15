
using BookShop.Models;
using BookShop.parser;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly BookManager manager;


        public HomeController(BookManager _manager)
        {
            manager = _manager;
        }


        public IActionResult Index()
        {

            return View(manager.GetBooksList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult GetAction()
        {
            manager.AddBooksFrom(Parser.GetBooks(5));
            return View("Index", manager.GetBooksList());
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
                manager.AddBook(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }


        [HttpPost, ActionName("GetBookById")]
        public ActionResult DeleteConfirmed(int id)
        {
            manager.DeleteBook(id);
            return RedirectToAction("Index");
        }

        public ActionResult GetBookById(int id)
        {
            var book = manager.GetBookById(id);
            return View(book);
        }
        [HttpGet]
        public ActionResult SearchBooks(string Name, string Author, decimal Price)
        {
            FilterOptions.NameofBook = Name;
            FilterOptions.Author = Author;
            FilterOptions.Price = Price;
            var bookList = manager.GetBooksWithFilter();
            return View("Index", bookList);
        }

        [HttpGet]
        public ActionResult EditBook(int id)
        {
           var book = manager.GetBookById(id);
            return View(book);
        }
        
        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                manager.EditBook(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }
    }
}
