using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddBlogPostAsync(BlogPost blogPost)
        {
            await bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteBlogPostByIdAsync(Guid Id)
        {
            var DeleteBlogPost=await bloggieDbContext.BlogPosts.FirstOrDefaultAsync(x=>x.Id==Id);
            if (DeleteBlogPost != null) {
                bloggieDbContext.BlogPosts.Remove(DeleteBlogPost);
                await bloggieDbContext.SaveChangesAsync();
                return DeleteBlogPost;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BlogPost>> GetAllAsync()
        {
            var getAllBlogPost = await bloggieDbContext.BlogPosts
                .Include(x=>x.Tags)
                .OrderBy(x => x.Heading)
                .ToListAsync();
            return getAllBlogPost;
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(Guid Id)
        {
            var getBlogById = await bloggieDbContext.BlogPosts
                .Include(x=>x.Tags)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if(getBlogById is not null)
            {
                return getBlogById;
            }
            else
            {
                return null;
            }

        }

        public async Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateBlogPostByIdAsync(BlogPost blogPost)
        {
            var databyId = await bloggieDbContext.BlogPosts
                .Include(x=>x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if(databyId != null)
            {
                databyId.Heading = blogPost.Heading;
                databyId.PageTitle = blogPost.PageTitle;
                databyId.Content = blogPost.Content;
                databyId.ShortDescription = blogPost.ShortDescription;
                databyId.UrlHandle = blogPost.UrlHandle;
                databyId.FeaturedImageUrl= blogPost.FeaturedImageUrl;
                databyId.PublishDate = blogPost.PublishDate;    
                databyId.Author = blogPost.Author;
                databyId.Visible = blogPost.Visible;    
                databyId.Tags= blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return databyId;
            }
            else
            {
                return null;
            }
        }
    }
}
