using BlazorInputFile;

namespace ProjectManager.Web.FileServices.Interfaces;

public interface IImageService
{
  Task UploadImage(IFileListEntry file, string fileName);
}
