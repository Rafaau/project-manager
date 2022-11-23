using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ProjectManager.IntegrationTests;
public class ApiServer : IDisposable
{
  private WireMockServer _server;

  public string Url => _server.Url!;
  public void Start()
  {
    _server = WireMockServer.Start();
  }

  public void SetupUser(string username)
  {
    _server.Given(Request.Create()
      .WithPath($"/users/{username}")
      .UsingGet())
      .RespondWith(Response.Create()
        .WithBody($@"{{
          ""user"": ""{username}"",
          ""message"": ""Successful user setup""
        }}")
        .WithHeader("content-type", "application/json; charset=tf-8")
        .WithStatusCode(200));
  }

  public void Dispose()
  {
    _server.Stop();
    _server.Dispose();
  }
}
