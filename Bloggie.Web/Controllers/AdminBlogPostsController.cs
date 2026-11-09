using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public AdminBlogPostsController(IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var getTags = await tagRepository.GetAllTagsAsync();
            var addmodel = new AddBlogPostRequest
            {
                Tags = getTags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(addmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPostDomin = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishDate = addBlogPostRequest.PublishDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            //map Tags from selected tags
            var selectedTagList = new List<Tags>();
            foreach (var items in addBlogPostRequest.SelectedTags)
            {
                var selectedtagId = Guid.Parse(items);
                var existingTag = await tagRepository.GetTagByIdAsync(selectedtagId);
                if (existingTag != null)
                {
                    selectedTagList.Add(existingTag);
                }
            }

            blogPostDomin.Tags = selectedTagList;

            await blogPostRepository.AddBlogPostAsync(blogPostDomin);
            return RedirectToAction("List", "AdminBlogPosts");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var getAllBlogPostData = await blogPostRepository.GetAllAsync();
            return View(getAllBlogPostData);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var blogPostbyId = await blogPostRepository.GetBlogPostByIdAsync(Id);
            var tagsDomin= await tagRepository.GetAllTagsAsync();   

            if (blogPostbyId != null)
            {
                var blogPostDto = new EditBlogPostRequest
                {
                    Id = blogPostbyId.Id,
                    Heading = blogPostbyId.Heading,
                    PageTitle = blogPostbyId.PageTitle,
                    Content = blogPostbyId.Content,
                    ShortDescription = blogPostbyId.ShortDescription,
                    UrlHandle = blogPostbyId.UrlHandle,
                    FeaturedImageUrl = blogPostbyId.FeaturedImageUrl,
                    PublishDate = blogPostbyId.PublishDate,
                    Author = blogPostbyId.Author,
                    Visible = blogPostbyId.Visible,
                    Tags = tagsDomin.Select(x => new SelectListItem
                    {
                        Text=x.Name,
                        Value=x.Id.ToString()
                    }),
                    SelectedTags=blogPostbyId.Tags.Select(x=>x.Id.ToString()).ToArray(),
                };
                

                return View(blogPostDto);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomin = new BlogPost
            {
                Id=editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishDate = editBlogPostRequest.PublishDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };
            //map Tags from selected tags
            var selectedTagList = new List<Tags>();
            foreach (var items in editBlogPostRequest.SelectedTags)
            {
                var selectedtagId = Guid.Parse(items);
                var existingTag = await tagRepository.GetTagByIdAsync(selectedtagId);
                if (existingTag != null)
                {
                    selectedTagList.Add(existingTag);
                }
            }

            blogPostDomin.Tags = selectedTagList;

            var updatedblog  = await blogPostRepository.UpdateBlogPostByIdAsync(blogPostDomin);
            if(updatedblog != null)
            {
                return RedirectToAction("List", "AdminBlogPosts");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingblogs = await blogPostRepository.DeleteBlogPostByIdAsync(id);
            if (existingblogs is not null)
            {
                return RedirectToAction("List", "AdminBlogPosts");
            }
            return RedirectToAction("Edit", "AdminBlogPosts");
        }

    }
}
