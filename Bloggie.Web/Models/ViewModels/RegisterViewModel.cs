using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Bloggie.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6,ErrorMessage ="Password has to be atleast 6 characters with a combination of alphanumeric, special chars, uppercase, lowercase, numeric")]
        public string Password { get; set; }
    }
}
