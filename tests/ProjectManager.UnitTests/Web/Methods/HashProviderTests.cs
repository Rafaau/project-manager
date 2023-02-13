using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProjectManager.Web.Authentication;
using Xunit;

namespace ProjectManager.UnitTests.Web.Methods;
public class HashProviderTests
{
  [Fact]
  public async Task HashPassword_ShouldReturnEncryptedString_WhenSucceeded()
  {
    // Arrange
    var expected = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==";

    // Act
    var result = "pm2022".HashPassword();
    var decrypted = result.VerifyHashedPassword("pm2022");

    // Assert
    decrypted.Should().BeTrue();
  }
}
