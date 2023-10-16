using Bloggie.Web.Repositories.ImageRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
		[HttpGet]
		public IActionResult Index()
		{
			return Ok("This is the GET Images API call 🙌");
		}
		[HttpPost]
		public async Task<IActionResult> UploadAsync(IFormFile file)
		{
			//call a repository
			var imageURL = await imageRepository.UploadAsync(file);
			if (imageURL == null)
			{
				return Problem("Something went wron!", null, (int)HttpStatusCode.InternalServerError);
			}
			return new JsonResult(new { link = imageURL});
		}
	}
}
