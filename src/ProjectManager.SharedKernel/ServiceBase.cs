namespace ProjectManager.SharedKernel;
public abstract class ServiceBase
{
  protected readonly HttpClient HttpClient;

  public ServiceBase(IHttpClientFactory httpClientFactory)
  {
    HttpClient = httpClientFactory.CreateClient("api");
  }
}
