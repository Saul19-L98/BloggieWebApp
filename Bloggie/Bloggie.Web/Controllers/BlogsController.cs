using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.BlogPostCommentRepository;
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
		private readonly IBlogPostCommentReposirtory blogPostCommentReposirtory;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public BlogsController(IBlogPostsRepository blogPostsRepository, IBlogPostLikeRepository blogPostLikeRepository,IBlogPostCommentReposirtory blogPostCommentReposirtory, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) 
        {
            this.blogPostsRepository = blogPostsRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.blogPostCommentReposirtory = blogPostCommentReposirtory;
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
                //Get comments for blog post
                var blogComments = await blogPostCommentReposirtory.GetCommentsByIdAsync(blogPost.Id);
                
                var blogPostCommentsForView = new List<BlogComment>(blogComments.Count());
                foreach (var blogComment in blogComments)
                {
                    blogPostCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString()))!.UserName!,
                    });
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
					Comments = blogPostCommentsForView
				};
				return View(blogPostLikeViewModel);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var blogPostComment = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)!),
                    DateAdded = DateTime.Now,
                };
                await blogPostCommentReposirtory.AddAsync(blogPostComment);
                return RedirectToAction("Index","Blogs",new {urlHandle = blogDetailsViewModel.UrlHandle});
            }
            return View();
		}
    }
}
