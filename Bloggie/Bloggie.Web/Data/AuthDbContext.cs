using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data;

public class AuthDbContext : IdentityDbContext
{
    private readonly IConfiguration configuration;
    public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Seed roles (user,admin,superAdmin)
        var adminRoleId =  Guid.NewGuid().ToString();
        var superAdminRoleId = Guid.NewGuid().ToString();
        var userRoleId = Guid.NewGuid().ToString();
        
        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },
            new IdentityRole
            {
                Name="SuperAdmin",
                NormalizedName="SuperAdmin",
                Id=superAdminRoleId,
                ConcurrencyStamp=superAdminRoleId
            },
            new IdentityRole
            {
                Name="User",
                NormalizedName="User",
                Id=userRoleId,
                ConcurrencyStamp=userRoleId
            }
        };

        //When this line gets executed Entity framework will take care of this and when it runs this line it will insert this roles inside the database.
        builder.Entity<IdentityRole>().HasData(roles);

        //Seed superAdminUser
        var superAdminUserId = Guid.NewGuid().ToString();
        var superAdminUser = new IdentityUser
        {
            UserName = "superadmin@bloggie.com",
            Email = "superadmin@bloggie.com",
            NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
            NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
            Id = superAdminUserId
        };
        
        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, configuration.GetValue<string>("SuperAdminUserPassword")!);
        builder.Entity<IdentityUser>().HasData(superAdminUser);

        //Add All the roles to superAdminUser
        var superAdminRole = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdminUserId,
            },
            new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminUserId
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = superAdminUserId
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRole);
    }
}
