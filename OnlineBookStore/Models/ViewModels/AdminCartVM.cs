using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models.ViewModels
{
    public class AdminCartVM
    {
        public string UserID { get; set; }
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Display(Name = "Cart Items")]
        public int CartItemCount { get; set; } = 0;
    }
}
