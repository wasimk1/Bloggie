using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);

        Task<BlogPostLike> AddLikesToBlogsAsync(BlogPostLike blogPostLike);

        Task<List<BlogPostLike>> GetLikesForBlogAsync (Guid blogPostId);
    }
}
