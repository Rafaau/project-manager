using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace ProjectManager.UnitTests;
public static class RequestFactory
{
  public static HttpRequest Create(IEdmModel model, ODataPath path)
  {
    HttpContext context = new DefaultHttpContext();
    context.ODataFeature().Model = model;
    context.ODataFeature().Path = path;
    return context.Request;
  }
}
