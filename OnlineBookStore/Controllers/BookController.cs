using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {
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
            return View(); //unimplemented
        }
    }
}
