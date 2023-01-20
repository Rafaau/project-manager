using BlazorInputFile;
using ProjectManager.Web.FileServices.Interfaces;

namespace ProjectManager.Web.FileServices.Services;

public class ImageService : IImageService
{
  private readonly IWebHostEnvironment _environment;
  public ImageService(IWebHostEnvironment environment)
  {
    _environment = environment;
  }

  public async Task UploadImage(IFileListEntry file, string fileName)
  {
    var path = Path.Combine(_environment.ContentRootPath, "wwwroot/avatars", fileName);
    var memoryStream = new MemoryStream();
    await file.Data.CopyToAsync(memoryStream);

    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      memoryStream.WriteTo(fileStream);
    }
  }
}
