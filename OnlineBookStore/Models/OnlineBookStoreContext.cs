using Microsoft.EntityFrameworkCore;

namespace OnlineBookStore.Models
{
    //Context - model
    public class OnlineBookStoreContext:DbContext
    {
        public OnlineBookStoreContext(DbContextOptions<OnlineBookStoreContext> options)
        : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        /* table names must be plural */

        /* This will keep on changing
         * you will add new tables, modify existing ones, or remove tables */
        /* System to track these changes - Migration system 
         1) Create an Initial Migration (Database State)*/
    }
}
