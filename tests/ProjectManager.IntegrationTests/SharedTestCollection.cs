using Xunit;

namespace ProjectManager.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<ApiFactory>
{
}
