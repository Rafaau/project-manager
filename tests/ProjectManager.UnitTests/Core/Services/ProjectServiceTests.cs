using Ardalis.Specification;
using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel.Interfaces;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Core.Services;
public class ProjectServiceTests
{
  private readonly IProjectService _sut;
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly ILoggerAdapter<ProjectService> _logger = Substitute.For<ILoggerAdapter<ProjectService>>();

  public ProjectServiceTests()
  {
    _sut = new ProjectService(_projectRepository, _userRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllProjects_ShouldReturnEmptyList_WhenNoProjectsExist()
  {
    // Arrange
    _projectRepository.ListAsync()
      .Returns(new List<Project2>());

    // Act
    var result = await _sut.RetrieveAllProjects();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllProjects_ShouldReturnProjects_WhenSomeProjectsExist()
  {
    // Arrange
    var expectedProjects = FakeProjectsList();
    _projectRepository.ListAsync()
      .Returns(expectedProjects);

    // Act
    var result = await _sut.RetrieveAllProjects();

    // Assert
    result.Should().BeEquivalentTo(expectedProjects);
  }

  [Fact]
  public async Task RetrieveAllProjects_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _projectRepository.ListAsync()
      .Returns(new List<Project2>());

    // Act
    await _sut.RetrieveAllProjects();

    // Assert
    _logger.Received(1).LogInformation("Retrieving all projects");
    _logger.Received(1).LogInformation("Projects retrieved in {0}ms", Arg.Any<long>());
  }

  [Fact]
  public async Task RetrieveAllProjects_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _projectRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllProjects();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving all projects"));
  }
  #endregion

  #region RetrieveById
  [Fact]
  public async Task RetrieveProjectById_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveProjectById(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving project (id: 1)"));
  }

  [Fact]
  public async Task RetrieveProjectById_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    var result = await _sut.RetrieveProjectById(1);

    // Assert
    result.Should().BeEquivalentTo(FakeProject());
  }

  [Fact]
  public async Task RetrieveProjectById_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    await _sut.RetrieveProjectById(1);

    // Assert
    _logger.Received(1).LogInformation("Retrieving project with id: {0}", 1);
    _logger.Received(1).LogInformation("Project (id: {0}) retrieved in {1}ms", 1, Arg.Any<long>());
  }
  #endregion

  #region Create
  [Fact]
  public async Task CreateProject_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateProject(FakeProject());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while creating project"));
  }

  [Fact]
  public async Task CreateProject_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var projectToAdd = FakeProject();
    _projectRepository.AddAsync(Arg.Any<Project2>())
      .Returns(projectToAdd);

    // Act
    var result = await _sut.CreateProject(projectToAdd);

    // Assert
    result.Should().Be(projectToAdd);
  }

  [Fact]
  public async Task CreateProject_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _projectRepository.AddAsync(Arg.Any<Project2>())
      .Returns(FakeProject());

    // Act
    await _sut.CreateProject(FakeProject());

    // Assert
    _logger.Received(1).LogInformation("Creating project");
    _logger.Received(1).LogInformation("Project created in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task UpdateProject_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.UpdateProject(FakeProject());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.LogError(Arg.Is(exception), Arg.Is("Something went wrong while updating project (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task UpdateProject_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    var result = await _sut.UpdateProject(FakeProject());

    // Assert
    result.Should().BeEquivalentTo(FakeProject());
  }

  [Fact]
  public async Task UpdateProject_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    await _sut.UpdateProject(FakeProject());

    // Assert
    _logger.Received(1).LogInformation("Updating project (id: {0})", 1);
    _logger.Received(1).LogInformation("Project (id: {0}) updated in {1}ms", 1, Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteProject_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteProject(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while deleting project (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task DeleteProject_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    var result = await _sut.DeleteProject(1);

    // Assert
    result.Should().BeEquivalentTo(FakeProject());
  }

  [Fact]
  public async Task DeleteProject_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    await _sut.DeleteProject(1);

    // Assert
    _logger.Received(1).LogInformation("Deleting project (id: {0})", Arg.Is(1));
    _logger.Received(1).LogInformation("Project (id: {0}) deleted in {1}ms", Arg.Is(1), Arg.Any<long>());
  }
  #endregion
}
