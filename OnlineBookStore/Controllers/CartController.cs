using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using OnlineBookStore.Models;
using OnlineBookStore.Models.ViewModels;
using System.Xml;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineBookStoreContext obc;
        private readonly UserManager<WebUser> u;

        public CartController(OnlineBookStoreContext o, UserManager<WebUser> um)
        {
            obc = o;
            u = um;
        }


        public async Task<int> GetItemCount(string userid){
            var CurrentUser = await u.FindByIdAsync(userid);
            int count =(from x in obc.Carts
                       where x.WebUser.Id == CurrentUser.Id.ToString()
                       select x).Count();
                return count;
            }
        public async Task<IActionResult> AddToCart(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound("Book ID not provided or does not exist");
            }
            Book bk = await obc.Books.FindAsync(cartItemId) ;
            if(bk == null)
            {
                return NotFound("Book ID not provided or does not exist");
            }
            string userID = u.GetUserId(User); //gets the current User ID

            if (string.IsNullOrEmpty(userID))
            {
                return RedirectToAction("WebUser", "Login");
            }
            obc.Carts.Add(new Cartitem() { 
                BookId = cartItemId.Value,
                WebUserId = userID,
                AddTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            });
            await obc.SaveChangesAsync();
            return RedirectToAction("Carts", "ListAll");

            
        }
        
        public async Task<IActionResult> RemoveFromCart(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound("Invalid Request");
            }
            Cartitem ci = await obc.Carts.FindAsync(cartItemId);
            if (ci == null)
            {
                return NotFound("Book is not present in your cart");
            }
            obc.Carts.Remove(ci);
            await obc.SaveChangesAsync();

            return RedirectToAction("Carts", "ListAll");
        }

        /*
            graph TD
                A[Start] --> B{Is uid null or empty?}
                B -->|Yes| C[Set userID to u.GetUserId-User]
                B -->|No| D{Is User in Admin role?}
                D -->|Yes| E[Query DB for uid]
                E --> F{Is uidDB null?}
                F -->|Yes| G[Return NotFound]
                F -->|No| H[Proceed to list books]
                D -->|No| I[Return BadRequest]
                C --> J[Join Carts and Books, filter by userID]
                H --> J
                J --> K[Return View with books]
         */
        /// <summary>
        /// Gets Books in the users cart, If admin want's he can see the books in a specific users cart
        /// </summary>
        /// <param name="uid">The userid for which Books are to be shown</param>
        /// <returns>List of books async</returns>

        [HttpGet]
        
        public async Task<IActionResult> GetCartItems(string? uid = null)
        {
            string userID = "";
            if (string.IsNullOrEmpty(uid))
            {
               userID = u.GetUserId(User);
            }
            else
            {
                if (User.IsInRole("Admin"))
                {


                    string uidDB = obc.Users.Where(x => x.Id == uid).FirstOrDefault().Id;

                    if (uidDB == null)
                    {
                        return NotFound("Invalid User ID");
                    }
                }
                else
                {
                    return BadRequest("Admin previleges required");
                }
            }
            List<CartItemVM> items = await (from c in obc.Carts
                                            join b in obc.Books
                                            on c.BookId equals b.BookId
                                            where c.WebUserId == userID
                                            select new CartItemVM
                                            {
                                                BookName = b.Title,
                                                CartItemID = c.CartItemId,
                                                Price = b.Price
                                            }).ToListAsync();

            return View(items);
                        

        }
        [HttpGet]
        
        public async Task<IActionResult> GetViewForAdmin()
        {
            //projection
            var items = from c in obc.Carts
                                       group c by c.WebUserId into ug
                                       join us in obc.Users
                                       on ug.FirstOrDefault().WebUserId equals us.Id
                                       select new AdminCartVM
                                       {
                                           UserID = us.Id,
                                           UserName = us.UserName,
                                           CartItemCount = ug.Count()
                                       };

            return View(await items.ToListAsync());


        }
    }
}
