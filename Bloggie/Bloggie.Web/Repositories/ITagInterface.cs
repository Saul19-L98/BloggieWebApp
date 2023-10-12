﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories;

public interface ITagInterface
{
	Task<IEnumerable<Tag>> GetAllAsync();
	Task<Tag> GetAsync(Guid id);
	Task<Tag> AddAsync(Tag tag);
	Task<Tag?> UpdateAsync(Tag tag);
	Task DeleteAsync(Guid id);
}
