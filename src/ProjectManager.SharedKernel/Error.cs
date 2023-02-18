namespace ProjectManager.SharedKernel;
public class Error
{
  public string Source { get; set; }
  public string Message { get; set; }
  public Error()
  {
  }

  public Error(string source, string message)
  {
    Source = source;
    Message = message;
  }
}
