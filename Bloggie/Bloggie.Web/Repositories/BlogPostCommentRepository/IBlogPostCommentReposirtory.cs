using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories.BlogPostCommentRepository;

public interface IBlogPostCommentReposirtory
{
	Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
	Task<IEnumerable<BlogPostComment>> GetCommentsByIdAsync(Guid blogPostId);
}
