
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

namespace Bloggie.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikesToBlogsAsync(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;    
        }

        public async Task<List<BlogPostLike>> GetLikesForBlogAsync(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLike.Where(x=>x.BlogPostId==blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            var totallikes= await bloggieDbContext.BlogPostLike.CountAsync(x=>x.BlogPostId == blogPostId);  
            return totallikes;
        }

    }
}
