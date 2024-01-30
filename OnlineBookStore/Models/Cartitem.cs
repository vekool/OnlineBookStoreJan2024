using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineBookStore.Models
{
    public class Cartitem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CartItemId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "AddTime")]

        [DataType(DataType.DateTime)]
        public string? AddTime { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [ForeignKey("WebUser")]
        public int WebUserId { get; set; }

        /* these properties will not be present in the database 
         navigation properties*/
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        public virtual WebUser WebUser { get; set; }

    }
}
