using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models.ViewModels
{
    public class CartItemVM
    {
        [Display(Name ="Book Name")]
        public string BookName { get; set; }
        public int CartItemID { get; set; }

        public double Price { get; set; }
        //any other things like price, author (which you want)
    }
}
