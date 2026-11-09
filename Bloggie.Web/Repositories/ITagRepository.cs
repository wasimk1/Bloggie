using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tags>> GetAllTagsAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, 
            int pageNumber = 1, int pageSize = 5);

        Task<Tags?> GetTagByIdAsync(Guid id);

        Task<Tags> AddTagsAsync(Tags tags);

        Task<Tags?> UpdateTagsAsync(Guid id, Tags tags);

        Task<Tags> DeleteTagByIdAsync(Guid id);

        Task<int> CountAsync();
    }
}
