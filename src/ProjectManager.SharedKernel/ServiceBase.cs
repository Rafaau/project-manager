using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProjectManager.SharedKernel;
public abstract class ServiceBase
{
  protected readonly HttpClient HttpClient;

  public ServiceBase(IHttpClientFactory httpClientFactory)
  {
    HttpClient = httpClientFactory.CreateClient("api");
  }
}
