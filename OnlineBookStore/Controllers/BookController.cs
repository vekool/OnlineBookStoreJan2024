using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {
        OnlineBookStoreContext odb;

        public BookController(OnlineBookStoreContext o)
        {
            odb = o;
        }
        public IActionResult Index()
        {

            return View(odb.Books.ToArray());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Book());
        }
        [HttpPost]
        public IActionResult Create([Bind(include:  "Title, Price, PDate, Author")] Book b)
        {
            if (ModelState.IsValid)
            {
                odb.Books.Add(b);
                odb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(b);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            Book b = odb.Books.Find(id.Value);
            if (b == null)
            {
                return NotFound();
            }
            return View(b);
           
        }
        [HttpPost]
        public IActionResult Edit(Book b)
        {
          
            Book bo = odb.Books.Find(b.BookId);
            if (b == null)
            {
                return NotFound();
            }
            odb.Books.Update(b);
            odb.SaveChanges();
            return RedirectToAction("Index");          

        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            Book b = odb.Books.Find(id.Value);
            if(b == null)
            {
                return NotFound();
            }
            return View(b);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            Book b = odb.Books.Find(id.Value);
            if (b == null)
            {
                return NotFound();
            }
            odb.Books.Remove(b);
            odb.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult View2()
        {
            return View();
        }
    }
}
