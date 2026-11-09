using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //call a cloudImagerepository
            var imageUrl = await imageRepository.UploadAsync(file);
            if (imageUrl == null) {
                return BadRequest("something was wrong");
            }
            return new JsonResult(new
            {
                link = imageUrl.ToString()
            });
        }
    }
}
