using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories.BlogPostCommentRepository;

public class BlogPostCommentReposirtory : IBlogPostCommentReposirtory
{
	private readonly BloggieDbContext bloggieDbContext;

	public BlogPostCommentReposirtory(BloggieDbContext bloggieDbContext)
	{
		this.bloggieDbContext = bloggieDbContext;
	}
    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
	{
		await bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
		await bloggieDbContext.SaveChangesAsync();
		return blogPostComment;
	}

	public async Task<IEnumerable<BlogPostComment>> GetCommentsByIdAsync(Guid blogPostId)
	{
		return await bloggieDbContext.BlogPostComment.Where(blog => blog.BlogPostId == blogPostId).ToListAsync();
	}
}
