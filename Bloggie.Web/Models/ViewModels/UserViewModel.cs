using System.Reflection.Metadata.Ecma335;

namespace Bloggie.Web.Models.ViewModels
{
    public class UserViewModel
    {
        public List<User> users { get; set; }
        public string Username { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
        public bool AdminRoleChkBox { get; set; }
    }
}
