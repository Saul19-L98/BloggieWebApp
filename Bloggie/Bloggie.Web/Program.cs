using Bloggie.Web.Data;
using Bloggie.Web.Repositories.BlogPostCommentRepository;
using Bloggie.Web.Repositories.BlogPostLikeRepository;
using Bloggie.Web.Repositories.BlogPostsRepository;
using Bloggie.Web.Repositories.ImageRepository;
using Bloggie.Web.Repositories.TagRepository;
using Bloggie.Web.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inject our DbContext class inside the services of our application using dependency injection.

builder.Services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

// Means: Add a injection inside the services. When sombody calls the I tag repository, give them the implementation instead of the interface.
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IBlogPostsRepository, BlogPostsRepository>();

builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();

builder.Services.AddScoped<IBlogPostCommentReposirtory, BlogPostCommentReposirtory>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
