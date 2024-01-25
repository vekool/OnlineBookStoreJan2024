using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {

        private readonly OnlineBookStoreContext odb;
        //framework
        public BookController(OnlineBookStoreContext o)
        {
            odb = o;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book b)
        {
            //recheck whether everything is ok
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Title", "Some error occurred");
                return View();
            }
            odb.Books.Add(b);
            odb.SaveChanges();
            return View(); //unimplemented
        }
        [HttpGet]
        public IActionResult ListAllBooks()
        {
            
            return View(odb.Books.ToList());
        }
    }
}
