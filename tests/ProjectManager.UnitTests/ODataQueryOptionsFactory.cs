using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;

namespace ProjectManager.UnitTests;
public static class ODataQueryOptionsFactory
{
  public static ODataQueryOptions<T> GetQueryOptions<T>() where T : class
  {
    var modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<T>(nameof(T));
    var edmModel = modelBuilder.GetEdmModel();

    IEdmEntitySet entitySet = edmModel.EntityContainer.FindEntitySet(nameof(T));
    ODataPath path = new ODataPath(new EntitySetSegment(entitySet));

    var request = RequestFactory.Create(edmModel, path);

    request.ODataFeature().Model = edmModel;
    request.ODataFeature().Path = path;

    var oDataQueryContext = new ODataQueryContext(edmModel, typeof(T), new ODataPath());
    var oDataQueryOptions = new ODataQueryOptions<T>(oDataQueryContext, request);

    return oDataQueryOptions;
  }
}
