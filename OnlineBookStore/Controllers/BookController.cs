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
        public ActionResult Delete(int? id)
        {
            //check if id is null - error
            if (!id.HasValue)
            {
                throw new ArgumentException("No id provided");
            }

            //check if the book exists - if not then error
           Book b =  (from b in odb.Books
             where b.BookId == id.Value
             select b).SingleOrDefault();

            if(b == null)
            {
                throw new ArgumentException("No book found");
            }
            //Delete the book using remove
            odb.Books.Remove(b);
            odb.SaveChanges();
            return RedirectToAction("ListAllBooks");
            
        }
    }
}
