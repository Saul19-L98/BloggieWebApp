using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories.UserRepository;

public interface IUserRepository
{
	Task<IEnumerable<IdentityUser>> GetAllAsync();
	Task<string> GetUserRoleAsync(string id);
}
