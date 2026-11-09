using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            bool isliked = false;
            var blogPost = await blogPostRepository.GetBlogPostByUrlHandleAsync(urlHandle);
            var blogPostDetailsViewModel = new BlogDetailsViewModel();
            if (blogPost != null)
            {
                var totallikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    //Get Likes for this blog for this user
                    var alllikes= await blogPostLikeRepository.GetLikesForBlogAsync(blogPost.Id);
                    var userId = userManager.GetUserId(User);

                    if(userId != null)
                    {
                        var likeFromUser= alllikes.FirstOrDefault(x=>x.UserId==Guid.Parse(userId));
                        isliked = likeFromUser != null ? true : false;
                    }
                }

                //Get comments for Blogs post
                var blogscommentsdomainModel= await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentforView = new List<BlogComment>();   
                foreach(var blogComments in blogscommentsdomainModel)
                {
                    blogCommentforView.Add(new BlogComment
                    {
                        Description = blogComments.Description,
                        DateAdded = blogComments.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComments.UserId.ToString())).UserName,
                    });
                }


                blogPostDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishDate = blogPost.PublishDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = urlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLIkes= totallikes,
                    Liked= isliked,
                    Comments=blogCommentforView
                };
                return View(blogPostDetailsViewModel);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainmodel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded= DateTime.Now,
                };
                await blogPostCommentRepository.AddAsync(domainmodel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }
            return Forbid();
        }
    }
}
