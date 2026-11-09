using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            //Map addtagRequest to Tag domain model
            var tagDomainModel = new Tags
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddTagsAsync(tagDomainModel);

            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection,
            int pageNumber = 1, int pageSize = 5)
        {
            //this is directly from database without repositories pattern
            //var GetAllTagsList = await bloggieDbContext.Tags.ToListAsync();
            //return View(GetAllTagsList);

            //with repositories pattern

            var totalRecords = await tagRepository.CountAsync();
            if (pageSize > 10 || pageSize < 1)
            {
                pageSize = 5;
            }
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber = Convert.ToInt32(totalPages);
            }
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.searchQuery = searchQuery;
            ViewBag.sortBy = sortBy;
            ViewBag.sortDirection = sortDirection;
            ViewBag.totalPages = totalPages;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            var getAllTags = await tagRepository.GetAllTagsAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);
            return View(getAllTags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var taketags = await tagRepository.GetTagByIdAsync(Id);
            if (taketags != null)
            {
                var edittagsrequest = new EditTagRequest
                {
                    Id = taketags.Id,
                    Name = taketags.Name,
                    DisplayName = taketags.DisplayName
                };
                return View(edittagsrequest);
            }
            else
            {
                return View(null);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, EditTagRequest editTagRequest)
        {
            var editTagsDomain = new Tags
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatetags = await tagRepository.UpdateTagsAsync(Id, editTagsDomain);

            if (updatetags != null)
            {
                //show success notification
            }
            else
            {
                //show error notification
            }
            return RedirectToAction("List", "AdminTags");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var existingtags = await tagRepository.DeleteTagByIdAsync(Id);
            if (existingtags is not null)
            {
                return RedirectToAction("List", "AdminTags");
            }
            return RedirectToAction("Edit", "AdminTags");

        }
    }
}
