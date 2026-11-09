using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<List<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetBlogPostByIdAsync(Guid Id);

        Task<BlogPost> AddBlogPostAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateBlogPostByIdAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteBlogPostByIdAsync(Guid Id);

        Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle);
    }
}
