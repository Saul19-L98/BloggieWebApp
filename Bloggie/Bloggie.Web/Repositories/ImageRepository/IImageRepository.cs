namespace Bloggie.Web.Repositories.ImageRepository;

public interface IImageRepository
{
    Task<string?> UploadAsync(IFormFile file);
}
