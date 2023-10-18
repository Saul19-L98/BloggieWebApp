using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Bloggie.Web.Repositories.BlogPostsRepository;
using Bloggie.Web.Repositories.TagRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostsRepository blogPostsRepository;
		private readonly ITagRepository tagRepository;

		public HomeController(ILogger<HomeController> logger,IBlogPostsRepository blogPostsRepository,ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostsRepository = blogPostsRepository;
			this.tagRepository = tagRepository;
		}

        public async Task<IActionResult> Index()
        {
            //Getting all blogs
            var blogPosts = await blogPostsRepository.GetAllAsync();
            //getting all tags
            var tags = await tagRepository.GetAllAsync();

            if (blogPosts == null || tagRepository == null)
            {
                return View(null);
            }

            var homeView = new HomeViewModel { BlogPosts = blogPosts,Tags = tags };

            return View(homeView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}