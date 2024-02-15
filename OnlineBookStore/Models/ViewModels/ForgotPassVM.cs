using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models.ViewModels
{
    public class ForgotPassVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
