using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories.BlogPostLikeRepository;

public class BlogPostLikeRepository : IBlogPostLikeRepository
{
	private readonly BloggieDbContext bloggieDbContext;

	public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
    {
		this.bloggieDbContext = bloggieDbContext;
	}

	public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
	{
		await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
		await bloggieDbContext.SaveChangesAsync();
		return blogPostLike;
	}

	public async Task<IEnumerable<BlogPostLike>> GetLikesFromBlog(Guid blogPostId)
	{
		return await bloggieDbContext.BlogPostLike.Where(like => like.BlogPostId == blogPostId).ToListAsync();
	}

	public async Task<int> GetTotalLikes(Guid blogPostId)
	{
		return await bloggieDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);

	}
}
