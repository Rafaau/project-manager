namespace ProjectManager.SharedKernel;
public static class ResponseExtensions
{
  public static Response<T> ErrorResponse<T>(this string error)
  {
    return new Response<T>
    {
      Errors = new[] { new Error("error", error) }
    };
  }

  public static Response<T> Success<T>(this T data)
  {
    return new Response<T>
    {
      Data = data
    };
  }

  public static Response<T> AsResponse<T>(this IEnumerable<Error> errors)
  {
    return new Response<T>
    {
      Errors = errors
    };
  }

  public static Response<T> AsResponse<T>(this Exception exception)
  {
    var error = exception.AsErrors();
    return error.AsResponse<T>();
  }

  public static Response<T> AsResponse<T>(this Error error)
  {
    return new Response<T>
    {
      Errors = new[] { error }
    };
  }

  private static IEnumerable<Error> AsErrors(this Exception exception)
  {
    return new List<Error> { new Error(exception.Source, exception.Message) };
  }
}
