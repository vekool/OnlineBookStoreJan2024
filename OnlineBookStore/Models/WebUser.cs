using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace OnlineBookStore.Models   
{
    public class WebUser:IdentityUser
        //Model --> Database
        //ViewModel --> View 
        //View <---> ViewModel <--> Model <---> Database
    {
        public string? Name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public string? Dob { get; set; }
                
        [Required(ErrorMessage = "Address  is mandatory")]
        [Display(Name = "Primary Address")]
        public string? Address { get; set; }        
    }
}