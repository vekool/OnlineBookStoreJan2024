using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {
        OnlineBookStoreContext odb;

          UserManager<WebUser> userMan;
        SignInManager<WebUser> signInMan;
        public BookController(OnlineBookStoreContext o, UserManager<WebUser> um, SignInManager<WebUser> s)
        {
            odb = o;
            userMan = um;
            signInMan = s;
        }
        [Authorize]
        public IActionResult Index()
        {
            if(User.IsInRole("Admin"))

                return View(odb.Books.ToArray());
            else
            {
                return View(odb.Books.ToArray());
                //code to return only books issued to user
            }
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View(new Book());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public ActionResult View2()
        {
            return View();
        }
    }
}
