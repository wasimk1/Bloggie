using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<List<Tags>> GetAllTagsAsync(string? searchQuery, string? sortBy, string? sortDirection, 
            int pageNumber = 1, int pageSize = 5)
        {
            //var getAllTags = await bloggieDbContext.Tags.ToListAsync();
            var getAllTags = bloggieDbContext.Tags.AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                getAllTags = getAllTags.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection,"Desc",StringComparison.OrdinalIgnoreCase);
                if(string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
                {
                    getAllTags = isDesc ? getAllTags.OrderByDescending(x => x.Name):getAllTags.OrderBy(x => x.Name); 
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    getAllTags = isDesc ? getAllTags.OrderByDescending(x => x.DisplayName) : getAllTags.OrderBy(x => x.DisplayName);
                }
            }

            //Pagination

            var takeRecordSet = (pageNumber - 1) * pageSize;
            getAllTags = getAllTags.Skip(takeRecordSet).Take(pageSize);


            return await getAllTags.ToListAsync();
        }
        public async Task<Tags> AddTagsAsync(Tags tags)
        {
            await bloggieDbContext.Tags.AddAsync(tags);
            await bloggieDbContext.SaveChangesAsync();
            return tags;
        }

        public async Task<Tags> DeleteTagByIdAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                bloggieDbContext.SaveChanges();
                return existingTag;
            }
            return null;
        }

        public Task<Tags?> GetTagByIdAsync(Guid id)
        {
            var getTagbyId = bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (getTagbyId != null)
            {
                return getTagbyId;
            }
            return null;
        }

        public async Task<Tags?> UpdateTagsAsync(Guid id, Tags tags)
        {
            var existingTag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTag != null)
            {
                existingTag.Name = tags.Name;
                existingTag.DisplayName = tags.DisplayName;
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();        
        }
    }
}
