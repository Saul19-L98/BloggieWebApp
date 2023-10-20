using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories.BlogPostLikeRepository;

public interface IBlogPostLikeRepository
{
	Task<int> GetTotalLikes(Guid blogPostId);
	Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
	Task<IEnumerable<BlogPostLike>>GetLikesFromBlog(Guid blogPostId);
}
