using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostsRepository : IBlogPostsRepository
{
	private readonly BloggieDbContext bloggieDbContext;

	public BlogPostsRepository(BloggieDbContext bloggieDbContext)
    {
		this.bloggieDbContext = bloggieDbContext;
	}

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
	{
		await bloggieDbContext.AddAsync(blogPost);
		await bloggieDbContext.SaveChangesAsync();
		return blogPost;	
	}

	public async Task<BlogPost?> DeleteAsync(Guid id)
	{
		var existingBlogPost = await bloggieDbContext.BlogPosts.FindAsync(id);
		if (existingBlogPost != null)
		{
			bloggieDbContext.BlogPosts.Remove(existingBlogPost);
			await bloggieDbContext.SaveChangesAsync();
			return existingBlogPost;
		}
		return null;
	}

	public async Task<IEnumerable<BlogPost>> GetAllAsync()
	{
		return await bloggieDbContext.BlogPosts.Include(blogPost => blogPost.Tags).ToListAsync(); 
	}

	public async Task<BlogPost?> GetAsync(Guid id)
	{
		return await bloggieDbContext.BlogPosts.Include(blogPost => blogPost.Tags).FirstOrDefaultAsync(blogPost => blogPost.Id == id);
	}

	public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
	{
		var existingBlogPost = await bloggieDbContext.BlogPosts.Include(tag => tag.Tags).FirstOrDefaultAsync(blog => blog.Id == blogPost.Id);
		if (existingBlogPost != null)
		{
			existingBlogPost.Id = blogPost.Id;
			existingBlogPost.Heading = blogPost.Heading;
			existingBlogPost.PageTitle = blogPost.PageTitle;
			existingBlogPost.Content = blogPost.Content;
			existingBlogPost.ShortDescription = blogPost.ShortDescription;
			existingBlogPost.Author = blogPost.Author;
			existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
			existingBlogPost.UrlHandle = blogPost.UrlHandle;
			existingBlogPost.Visible = blogPost.Visible;
			existingBlogPost.PublishedDate = blogPost.PublishedDate;
			existingBlogPost.Tags = blogPost.Tags;

			await bloggieDbContext.SaveChangesAsync();
			return existingBlogPost;
		}
		return null;
	}
}
