using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookStore.Models
{
    //Context - model
    public class OnlineBookStoreContext:IdentityDbContext<WebUser, IdentityRole, string>
    {
        public OnlineBookStoreContext(DbContextOptions<OnlineBookStoreContext> options)
        : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<WebUser> WebUsers{ get; set; }

        public DbSet<Cartitem> Carts { get; set; }
        /* table names must be plural */

        /* This will keep on changing
         * you will add new tables, modify existing ones, or remove tables */
        /* System to track these changes - Migration system 
         1) Create an Initial Migration (Database State)*/
    }
}
