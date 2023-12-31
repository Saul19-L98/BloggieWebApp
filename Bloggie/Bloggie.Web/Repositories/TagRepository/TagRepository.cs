﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories.TagRepository;

public class TagRepository : ITagRepository
{
    private readonly BloggieDbContext bloggieDbContext;

    public TagRepository(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext;
    }

    public async Task AddAsync(Tag tag)
    {
        await bloggieDbContext.Tags.AddAsync(tag);
        await bloggieDbContext.SaveChangesAsync();
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var tag = await bloggieDbContext.Tags.FindAsync(id);
        if (tag != null)
        {
            bloggieDbContext.Tags.Remove(tag);
            bloggieDbContext.SaveChanges();
            return tag;
        }
        return null;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        var tags = await bloggieDbContext.Tags.ToListAsync();
        return tags;
    }

    public async Task<Tag?> GetAsync(Guid id)
    {
        var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(tag => tag.Id == id);
        return tag;
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            //save changes
            bloggieDbContext.SaveChanges();
            return existingTag;
        }
        return null;
    }
}
