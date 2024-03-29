﻿using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookControllerOLD : Controller
    {

        OnlineBookStoreContext odb;
        //framework
        public BookControllerOLD(OnlineBookStoreContext o)
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
            return RedirectToAction("ListAllBooks");
        }
        [HttpGet]
        public IActionResult ListAllBooks()
        {

            return View(odb.Books.ToList());
        
        }
        //Book/Edit/3
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //check if id is null - error
            if (!id.HasValue)
            {
                throw new ArgumentException("No id provided");
            }

            //check if the book exists - if not then error
            Book bo = (from b in odb.Books
                       where b.BookId == id.Value
                       select b).FirstOrDefault();
            //SIngleOrDefault
            //check if there is only a single book -
            // multiple books - exception
            // if not found - null 

            //FIrstorDefault
            //multiple books - get the first one
            //not found - null (default value)

            if (bo == null)
            {
                throw new ArgumentException("No book found");
            }

            return View(bo);

        }
        [HttpPost]
        public ActionResult Edit(Book newbook)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            odb.Update(newbook);
            odb.SaveChanges();
            return RedirectToAction("ListAllBooks");
            
        }
    }
}
