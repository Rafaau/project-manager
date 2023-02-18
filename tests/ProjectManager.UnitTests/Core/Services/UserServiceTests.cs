using Ardalis.Specification;
using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel.Interfaces;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Core.Services;
public class UserServiceTests
{
  private readonly UserService _sut;
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

  public UserServiceTests()
  {
    _sut = new UserService(_userRepository, _projectRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllUsers_ShouldReturnEmptyList_WhenNoUsersExist()
  {
    // Arrange
    _userRepository.ListAsync()
      .Returns(new List<User>());

    // Act
    var result = await _sut.RetrieveAllUsers();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllUsers_ShouldReturnUsers_WhenSomeUsersExist()
  {
    // Arrange
    var expectedUsers = FakeUsersList();
    _userRepository.ListAsync()
      .Returns(expectedUsers);

    // Act
    var result = await _sut.RetrieveAllUsers();

    // Assert
    result.Should().BeEquivalentTo(expectedUsers);
  }

  [Fact]
  public async Task RetrieveAllUsers_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _userRepository.ListAsync()
      .Returns(new List<User>());

    // Act
    await _sut.RetrieveAllUsers();

    // Assert
    _logger.Received(1).LogInformation(Arg.Is("Retrieving all users"));
    _logger.Received(1).LogInformation(Arg.Is("Users retrieved in {0}ms"), Arg.Any<long>());
  }

  [Fact]
  public async Task RetrieveAllUsers_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllUsers();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving all users"));
  }
  #endregion

  #region RetrieveByEmail
  [Fact]
  public async Task RetrieveUserByEmail_ShouldReturnUser_WhenUserExist()
  {
    // Arrange
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(FakeUser());

    // Act
    var result = await _sut.RetrieveUserByEmail("");

    // Assert
    result.Should().BeEquivalentTo(FakeUser());
  }

  [Fact]
  public async Task RetrieveUserByEmail_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var expectedEmail = "test";
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(FakeUser());

    // Act
    await _sut.RetrieveUserByEmail(expectedEmail);

    // Assert
    _logger.Received(1).LogInformation("Retrieving user with email: {0}", expectedEmail);
    _logger.Received(1).LogInformation("User with email: {0} retrieved in {1}ms", expectedEmail, Arg.Any<long>());
  }

  [Fact]
  public async Task RetrieveUserByEmail_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var expectedEmail = "test";
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveUserByEmail(expectedEmail);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1)
      .LogError(Arg.Is(exception), "Something went wrong while retrieving user with email: {0}", expectedEmail);
  }
  #endregion

  #region Create
  [Fact]
  public async Task CreateUser_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _userRepository.AddAsync(Arg.Any<User>())
      .Returns(FakeUser());

    // Act
    var result = await _sut.CreateUser(FakeUser());

    // Assert
    result.Should().BeEquivalentTo(FakeUser());
  }

  [Fact]
  public async Task CreateUser_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _userRepository.AddAsync(Arg.Any<User>())
      .Returns(FakeUser());

    // Act
    await _sut.CreateUser(FakeUser());

    // Assert
    _logger.Received(1).LogInformation("Creating user");
    _logger.Received(1).LogInformation("User created in {0}ms", Arg.Any<long>());
  }

  [Fact]
  public async Task CreateUser_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.AddAsync(Arg.Any<User>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateUser(FakeUser());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while creating user"));
  }
  #endregion

  #region UpdateUser
  [Fact]
  public async Task UpdateUser_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var userToUpdate = FakeUser();
    userToUpdate.Projects = FakeProjectsList();
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(userToUpdate);

    // Act
    var result = await _sut.UpdateUser(userToUpdate);

    // Assert
    result.Should().Be(userToUpdate);
  }

  [Fact]
  public async Task UpdateUser_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var userToUpdate = FakeUser();
    userToUpdate.Projects = FakeProjectsList();
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(userToUpdate);

    // Act
    await _sut.UpdateUser(userToUpdate);

    // Assert
    _logger.Received(1).LogInformation("Updating user (id: {0})", userToUpdate.Id);
    _logger.Received(1).LogInformation("User (id: {0}) updated in {1}ms", userToUpdate.Id, Arg.Any<long>());
  }

  [Fact]
  public async Task UpdateUser_ShouldLogMessage_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.UpdateUser(FakeUser());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while updating user (id: 1)"));
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteUser_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(FakeUser());

    // Act
    var result = await _sut.DeleteUser(1);

    // Assert
    result.Should().BeEquivalentTo(FakeUser());
  }

  [Fact]
  public async Task DeleteUser_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(FakeUser());

    // Act
    await _sut.DeleteUser(1);

    // Assert
    _logger.Received(1).LogInformation("Deleting user (id: {0})", 1);
    _logger.Received(1).LogInformation("Deleted user (id: {0}) in {1}ms", 1, Arg.Any<long>());
  }

  [Fact]
  public async Task DeleteUser_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteUser(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.LogError(Arg.Is(exception), "Something went wrong when deleting user (id: 1)");
  }
  #endregion
}
