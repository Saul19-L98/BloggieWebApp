using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
	private readonly AuthDbContext authDbContext;

	public UserRepository(AuthDbContext authDbContext)
    {
		this.authDbContext = authDbContext;
	}
    public async Task<IEnumerable<IdentityUser>> GetAllAsync()
	{
		var users = await authDbContext.Users.ToListAsync();
		var superAdminUser = await authDbContext.Users.FirstOrDefaultAsync(user => user.Email == "superadmin@bloggie.com");
		if (superAdminUser != null)
		{
			users.Remove(superAdminUser);
			return users;
		}
		return users;
	}

	public async Task<string> GetUserRoleAsync(string id)
	{
		var userRolesCodes = await authDbContext.UserRoles.Where(user => user.UserId == id).Join(authDbContext.Roles,
			userRole => userRole.RoleId,
			role => role.Id,
			(userRole,role)=> role.Name).ToArrayAsync();
		
		foreach (var role in userRolesCodes)
		{
			if (role == "Admin")
			{
				return "Admin";
			}
		}
		return "User";
	}
}
