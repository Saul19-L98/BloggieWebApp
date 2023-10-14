using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inject our DbContext class inside the services of our application using dependency injection.

builder.Services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

// Means: Add a injection inside the services. When sombody calls the I tag repository, give them the implementation instead of the interface.
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IBlogPostsRepository, BlogPostsRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
