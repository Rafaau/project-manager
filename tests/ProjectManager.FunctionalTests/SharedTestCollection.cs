using Xunit;

namespace ProjectManager.E2ETests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>
{
}
