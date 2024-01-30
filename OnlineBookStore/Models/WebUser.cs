using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class WebUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int WebUserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(4, ErrorMessage = "Name is too short")]
        [Display(Name = "Enter full name")]
        public string? Name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public string? Dob { get; set; }

        [Required(ErrorMessage = "E-mail  is mandatory")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile  is mandatory")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Address  is mandatory")]
        [Display(Name = "Primary Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Username is must be have 8 charater and atleast one symbol")]
        [MinLength(4, ErrorMessage = "UserName is too short")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is must be have 8 charater and atleast one symbol")]
        [MinLength(4, ErrorMessage = "Password is too short")]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[\W_]).{4,}$", ErrorMessage = "Password must contain at least one digit and one special character.")]
        public string? Pw { get; set; }

        
    }
}