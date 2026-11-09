using System.Reflection.Metadata;

namespace Bloggie.Web.Models.Domain
{
    public class Tags
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ICollection<BlogPost> BlogPost  { get; set; }
    }
}
