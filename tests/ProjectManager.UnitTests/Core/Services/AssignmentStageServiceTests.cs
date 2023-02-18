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
public class AssignmentStageServiceTests
{
  private readonly AssignmentStageService _sut;
  private readonly IRepository<AssignmentStage> _stageRepository = Substitute.For<IRepository<AssignmentStage>>();
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly ILoggerAdapter<AssignmentStageService> _logger = Substitute.For<ILoggerAdapter<AssignmentStageService>>();

  public AssignmentStageServiceTests()
  {
    _sut = new AssignmentStageService(_stageRepository, _projectRepository, _logger);
  }

  #region Create
  [Fact]
  public async Task CreateAssignmentStage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _stageRepository.AddAsync(Arg.Any<AssignmentStage>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.AddAssignmentStage(FakeStage());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.LogError(exception, "Something went wrong while creating assignment stage");
  }

  [Fact]
  public async Task CreateAssignmentStage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var stage = FakeStage();
    _stageRepository.AddAsync(Arg.Any<AssignmentStage>())
      .Returns(stage);

    // Act
    var result = await _sut.AddAssignmentStage(stage);

    // Assert
    result.Should().BeEquivalentTo(stage);
  }

  [Fact]
  public async Task CreateAssignmentStage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _stageRepository.AddAsync(Arg.Any<AssignmentStage>())
      .Returns(FakeStage());

    // Act
    await _sut.AddAssignmentStage(FakeStage());

    // Assert
    _logger.Received(1)
      .LogInformation(Arg.Is("Creating new assignment stage"));
    _logger.Received(1)
      .LogInformation(Arg.Is("Assignment stage created in {0}ms"), Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task UpdateAssignmentStage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.UpdateAssignmentStage(1, "Test");

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1)
      .LogError(exception,
        Arg.Is("Something went wrong while updating assignment stage (id: {0})"),
        Arg.Is(1));
  }

  [Fact]
  public async Task UpdateAssignmentStage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var stage = FakeStage();
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Returns(stage);

    // Act
    var result = await _sut.UpdateAssignmentStage(stage.Id, stage.Name);

    // Assert
    result.Should().BeEquivalentTo(stage);
  }

  [Fact]
  public async Task UpdateAssignmentStage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var stage = FakeStage();
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Returns(stage);

    // Act
    await _sut.UpdateAssignmentStage(stage.Id, stage.Name);

    // Assert
    _logger.Received(1)
      .LogInformation(Arg.Is("Updating assignment stage (id: {0})"), Arg.Is(stage.Id));
    _logger.Received(1)
      .LogInformation(Arg.Is("Assignment stage (id: {0}) updated in {1}ms"), Arg.Is(stage.Id), Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteAssignmentStage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteAssignmentStage(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(
      Arg.Is(exception),
      Arg.Is("Something went wrong while deleting assignment stage (id: {0})"),
      Arg.Is(1));
  }

  [Fact]
  public async Task DeleteAssignmentStage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var stage = FakeStage();
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Returns(stage);

    // Act
    var result = await _sut.DeleteAssignmentStage(stage.Id);

    // Assert
    result.Should().BeEquivalentTo(stage);
  }

  [Fact]
  public async Task DeleteAssignmentStage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var stage = FakeStage();
    _stageRepository.FirstOrDefaultAsync(Arg.Any<Specification<AssignmentStage>>())
      .Returns(stage);

    // Act
    await _sut.DeleteAssignmentStage(stage.Id);

    // Assert
    _logger.Received(1).LogInformation(Arg.Is("Deleting assignmment stage (id: {0})"), Arg.Is(stage.Id));
    _logger.Received(1).LogInformation(Arg.Is("Assignment stage (id: {0}) deleted in {1}ms"), Arg.Is(stage.Id), Arg.Any<long>());
  }
  #endregion
}
