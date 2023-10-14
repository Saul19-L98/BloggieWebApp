using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers;

public class AdminBlogPostsController : Controller
{
	private readonly ITagRepository tagRepository;
	private readonly IBlogPostsRepository blogPostsRepository;

	public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostsRepository blogPostsRepository)
    {
		this.tagRepository = tagRepository;
		this.blogPostsRepository = blogPostsRepository;
	}
    [HttpGet]
	public async Task<IActionResult> Add()
	{
		//get tags from repository
		var tags = await tagRepository.GetAllAsync();
		var model = new AddBlogPostsRequest
		{
			Tags = tags.Select(
				tag => new SelectListItem
				{
					Text = tag.Name,
					Value = tag.Id.ToString()
				}
			)
		};
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
	{
		//Map view model to domain model
		var blogPost = new BlogPost
		{
			Heading = addBlogPostsRequest.Heading,
			PageTitle = addBlogPostsRequest.PageTitle,
			Content = addBlogPostsRequest.Content,
			ShortDescription = addBlogPostsRequest.ShortDescription,
			FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
			UrlHandle = addBlogPostsRequest.UrlHandle,
			PublishedDate = addBlogPostsRequest.PublishedDate,
			Visible = addBlogPostsRequest.Visible,
			Author = addBlogPostsRequest.Author,

		};
		// Map Tags from selected tags
		var selectedTags = new List<Tag>();
		foreach (var selectedTagId in addBlogPostsRequest.SelectedTags)
		{
			var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
			var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
			if (existingTag != null)
			{
				selectedTags.Add(existingTag);
			}
		}

		//Maping tags back to domain model
		blogPost.Tags = selectedTags;

		var AddedBlogPost = await blogPostsRepository.AddAsync(blogPost);
		if(AddedBlogPost != null)
		{
			return RedirectToAction("List");
		}
		return RedirectToAction("Add", addBlogPostsRequest);
	}

	[HttpGet]
	public async Task<IActionResult> List()
	{
		//Call the repository
		var blogPosts = await blogPostsRepository.GetAllAsync();
		return View(blogPosts);
	}
	[HttpGet]
	public async Task<IActionResult> Edit(Guid id)
	{
		//Retrive the result from the repository
		var blogPost = await blogPostsRepository.GetAsync(id);
		var tagsDomainModel = await tagRepository.GetAllAsync();

		if (blogPost != null)
		{
			// Map the domain model in the view model
			var model = new EditBlogPostRequest
			{
				Id = blogPost.Id,
				Heading = blogPost.Heading,
				PageTitle = blogPost.PageTitle,
				Content = blogPost.Content,
				Author = blogPost.Author,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				UrlHandle = blogPost.UrlHandle,
				ShortDescription = blogPost.ShortDescription,
				PublishedDate = blogPost.PublishedDate,
				Visible = blogPost.Visible,
				Tags = tagsDomainModel.Select(tag => new SelectListItem
				{
					Value = tag.Id.ToString(),
					Text = tag.Name,
				}),
				SelectedTags = blogPost.Tags.Select(tag => tag.Id.ToString()).ToArray()
			};
			//pass data to view
			return View(model);
		}

		return View(null);
	}
	[HttpPost]
	public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
	{
		//map view model back to domain model 
		var blogPostDomainModel = new BlogPost
		{
			Id = editBlogPostRequest.Id,
			PageTitle = editBlogPostRequest.PageTitle,
			Content = editBlogPostRequest.Content,
			Author = editBlogPostRequest.Author,
			Heading = editBlogPostRequest.Heading,
			PublishedDate = editBlogPostRequest.PublishedDate,
			Visible = editBlogPostRequest.Visible,
			FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
			ShortDescription = editBlogPostRequest.ShortDescription,
			UrlHandle = editBlogPostRequest.UrlHandle,
		};
		//Map tags into domain model
		var selectedTags = new List<Tag>(editBlogPostRequest.SelectedTags.Length);
		foreach (var selectedTag in editBlogPostRequest.SelectedTags)
		{
			if (Guid.TryParse(selectedTag, out var tagId))
			{
				var foundTag = await tagRepository.GetAsync(tagId);
				if (foundTag != null)
				{
					selectedTags.Add(foundTag);
				}
			}
		}
		blogPostDomainModel.Tags = selectedTags;

		//Submit information to repository to update
		var updatedBlogPost = await blogPostsRepository.UpdateAsync(blogPostDomainModel);
		if (updatedBlogPost != null)
		{
			//Show success notification
			return RedirectToAction("List");
		}
		//Show error notification
		return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});

	}
	[HttpPost]
	public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
	{
		// Talk to repository to delete this blog post and tags
		var DeletedBlogPost = await blogPostsRepository.DeleteAsync(editBlogPostRequest.Id);
		if (DeletedBlogPost != null)
		{
			return RedirectToAction("List");
		}
		return RedirectToAction("Edit",new {id= editBlogPostRequest.Id});
	}
}
