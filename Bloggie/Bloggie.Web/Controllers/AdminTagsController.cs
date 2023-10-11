using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private BloggieDbContext _bloggieDbContext;

	public AdminTagsController(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    [ActionName("Add")]
    public IActionResult Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag { 
            Name = addTagRequest.Name, 
            DisplayName = addTagRequest.DisplayName 
        };
		_bloggieDbContext.Tags.Add(tag);
        _bloggieDbContext.SaveChanges();
        return RedirectToAction("List");
    }
    [HttpGet]
    [ActionName("List")]
    public IActionResult List()
    {
        //use dbContext to read the tags
        var tags = _bloggieDbContext.Tags.ToList();
        
        return View(tags);
    }
    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        // 1st method
        // var tag = _bloggieDbContext.Tags.Find(id);

        // 2nd method LINQ
       var tag = _bloggieDbContext.Tags.FirstOrDefault(tag => tag.Id == id);
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
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };
        var existingTag = _bloggieDbContext.Tags.Find(tag.Id);
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
    public IActionResult Delete(EditTagRequest editTagRequest)
    {
        var tag = _bloggieDbContext.Tags.Find(editTagRequest.Id);
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
