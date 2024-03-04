using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    //Option Error Message must be displayed (on the page via JS)
    //I must get suggestions (intelisense) on the page as well
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookId { get; set; }
        //annotation - Labels
        [Required(ErrorMessage ="Book Title is required")]
        //[MinLength(4, ErrorMessage ="Book title is too short")]
        [StringLength(100, MinimumLength =4, ErrorMessage = "Book title to small or too big")]
        [Display(Name ="Book Title")]
        public string? Title { get; set; }
       
        [Required(ErrorMessage = "Author name is required")]
        [MinLength(4, ErrorMessage = "Author Name is too short")]
        public string? Author { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Publish Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime PDate { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 50000, ErrorMessage ="Price too high or low")]
        public double Price { get; set; }

        //filename
        public string ImagePath { get; set; }
    }
}
