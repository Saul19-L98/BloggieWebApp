using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.BlogPostLikeRepository;
using Bloggie.Web.Repositories.BlogPostsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostsRepository blogPostsRepository;
		private readonly IBlogPostLikeRepository blogPostLikeRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public BlogsController(IBlogPostsRepository blogPostsRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) 
        {
            this.blogPostsRepository = blogPostsRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
		}
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await blogPostsRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost != null)
            {
                var liked = false;
                var tatalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    //Get lkie for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesFromBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
						var likeForBlog = likesForBlog.FirstOrDefault(liked => liked.UserId == Guid.Parse(userId));
                        liked = likeForBlog != null;
					}

                }

                var blogPostLikeViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = tatalLikes,
                    Liked = liked,
                };
				return View(blogPostLikeViewModel);
            }
            return View(null);
        }
    }
}
