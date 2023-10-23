using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.TagRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminTagsController : Controller
{
	private readonly ITagRepository tagRepository;

	public AdminTagsController(ITagRepository tagRepository)
    {
		this.tagRepository = tagRepository;
	}

	[HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        ValidateAddTagRequest(addTagRequest);
        if (ModelState.IsValid == false)
        {
            return View();
        }
		var tag = new Tag
		{
			Name = addTagRequest.Name,
			DisplayName = addTagRequest.DisplayName
		};
		await tagRepository.AddAsync(tag);
		return RedirectToAction("List");
    }

    [HttpGet]
    [ActionName("List")]
    public async Task<IActionResult> List()
    {
        //use dbContext to read the tags
        var tags = await tagRepository.GetAllAsync();
        
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // 1st method
        // var tag = _bloggieDbContext.Tags.Find(id);

        // 2nd method LINQ
        var tag = await tagRepository.GetAsync(id);
        if (tag != null)
        {
            var editTagRequest = new EditTagRequest
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };
            return View(editTagRequest);
        }
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };
        var existingTag = await tagRepository.UpdateAsync(tag);
        if (existingTag != null) 
        {
			// show success notification.
			return RedirectToAction("List", new { id = editTagRequest.Id }); ;
		}
        
		// show error notification.
		return RedirectToAction("Edit", new {id = editTagRequest.Id});
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var tag = await tagRepository.DeleteAsync(editTagRequest.Id);
        if(tag != null)
        {
            // Show a success notification
            return RedirectToAction("List");
        }
        // Show an error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id});
    }

    private void ValidateAddTagRequest(AddTagRequest request)
    {
        if (request.Name != null && request.DisplayName != null)
        {
            if (request.Name == request.DisplayName)
            {
                ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
            }
        }
    }
}
