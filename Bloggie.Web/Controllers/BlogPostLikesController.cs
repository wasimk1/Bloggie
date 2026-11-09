using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikesController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikesController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLIke([FromBody] AddLikeRequest addLikeRequest)
        {
            var blogLikeDomain = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId, 
                UserId = addLikeRequest.UserId
            };

            var addlikestodb=await blogPostLikeRepository.AddLikesToBlogsAsync(blogLikeDomain);
            if (addlikestodb != null) 
            { 
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{id:Guid}/totallikes")]
        public async Task<IActionResult> GetTotalLikesforBlog([FromRoute] Guid id)
        {
            var totallikes = await blogPostLikeRepository.GetTotalLikes(id);
            return Ok(totallikes);  
        }
    }
}
