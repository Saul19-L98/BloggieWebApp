using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private BloggieDbContext _bloggieDbContext;

	public AdminTagsController(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    private async Task SavingTags(Tag tag)
    {
		await _bloggieDbContext.Tags.AddAsync(tag);
		await _bloggieDbContext.SaveChangesAsync();
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
        var tag = new Tag { 
            Name = addTagRequest.Name, 
            DisplayName = addTagRequest.DisplayName 
        };
        await SavingTags(tag);
		return RedirectToAction("List");
    }

    [HttpGet]
    [ActionName("List")]
    public async Task<IActionResult> List()
    {
        //use dbContext to read the tags
        var tags = await _bloggieDbContext.Tags.ToListAsync();
        
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // 1st method
        // var tag = _bloggieDbContext.Tags.Find(id);

        // 2nd method LINQ
       var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(tag => tag.Id == id);
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
        var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            //save changes
            _bloggieDbContext.SaveChanges();
            // show success notification.
			return RedirectToAction("List", new { id = editTagRequest.Id }); ;
        }
		// show error notification.
		return RedirectToAction("Edit", new {id = editTagRequest.Id});
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var tag = await _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);
        if(tag != null)
        {
            _bloggieDbContext.Tags.Remove(tag);
            _bloggieDbContext.SaveChanges();
            // Show a success notification
            return RedirectToAction("List");
        }
        // Show an error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id});
    }
}
