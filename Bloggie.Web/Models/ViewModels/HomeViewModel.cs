using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; set; }
        public IEnumerable<Tags> Tags { get; set; }
    }
}
