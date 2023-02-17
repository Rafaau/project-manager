namespace ProjectManager.Infrastructure.Data.Config;
public static class AppDbContextOptions
{
  public static bool IsTesting { get; set; }
  public static bool IsE2ETesting { get; set; }
  public static string ConnectionString { get; set; } = "empty";
}
