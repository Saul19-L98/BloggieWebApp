using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories.TagRepository;

public interface ITagRepository
{
	Task<IEnumerable<Tag>> GetAllAsync();
	Task<Tag?> GetAsync(Guid id);
	Task AddAsync(Tag tag);
	Task<Tag?> UpdateAsync(Tag tag);
	Task<Tag?> DeleteAsync(Guid id);
}