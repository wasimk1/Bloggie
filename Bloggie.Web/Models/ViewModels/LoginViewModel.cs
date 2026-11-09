using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6,ErrorMessage = "Password has to be atleast 6 characters with a combination of alphanumeric, special chars, uppercase, lowercase, numeric")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }   
    }
}
