using Bloggie.Web.Repositories.BlogPostsRepository;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostsRepository blogPostsRepository;

        public BlogsController(IBlogPostsRepository blogPostsRepository) 
        {
            this.blogPostsRepository = blogPostsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await blogPostsRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost != null)
            {
                return View(blogPost);
            }
            return View(null);
        }
    }
}
