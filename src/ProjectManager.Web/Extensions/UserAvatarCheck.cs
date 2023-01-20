using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Extensions;

public static class UserAvatar
{
  public static bool CheckIfAvatarExist(this UserSimplified user, IWebHostEnvironment env)
  {
    return File.Exists(Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-{user.Id}.jpg"));
  }

  public static string GetAvatarPath(this UserSimplified user)
  {
    return $"/avatars/pm-avatar-{user.Id}.jpg";
  }
}
