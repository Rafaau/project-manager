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
public class AssignmentServiceTests
{
  private readonly AssignmentService _sut;
  private readonly IRepository<Assignment> _assignmentRepository = Substitute.For<IRepository<Assignment>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly IRepository<AssignmentStage> _stageRepository = Substitute.For<IRepository<AssignmentStage>>();
  private readonly ILoggerAdapter<AssignmentService> _logger = Substitute.For<ILoggerAdapter<AssignmentService>>();

  public AssignmentServiceTests()
  {
    _sut = new AssignmentService(_assignmentRepository, _projectRepository, _userRepository, _stageRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllAssignments_ShouldReturnEmptyList_WhenNoAssignmentsExist()
  {
    // Arrange
    _assignmentRepository.ListAsync().Returns(new List<Assignment>());

    // Act
    var result = await _sut.RetrieveAllAssignments();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllAssignments_ShouldReturnAssignments_WhenSomeAssignmentsExist()
  {
    // Arrange
    var expectedAssignments = FakeAssignmentsList();
    _assignmentRepository.ListAsync().Returns(expectedAssignments);

    // Act
    var result = await _sut.RetrieveAllAssignments();

    // Assert
    result.Should().BeEquivalentTo(expectedAssignments);
  }

  [Fact]
  public async Task RetrieveAllAssignments_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _assignmentRepository.ListAsync().Returns(new List<Assignment>());

    // Act
    await _sut.RetrieveAllAssignments();

    // Assert
    _logger.Received(1).LogInformation(Arg.Is("Retrieving all assignments"));
    _logger.Received(1).LogInformation(Arg.Is("All assignments retrieved in {0}ms"), Arg.Any<long>());
  }

  [Fact]
  public async Task RetrieveAllAssignments_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _assignmentRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllAssignments();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving all assignments"));
  }
  #endregion
  #region Create
  [Fact]
  public async Task CreateAssignment_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _assignmentRepository.AddAsync(Arg.Any<Assignment>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateAssignment(FakeAssignment());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Any<NpgsqlException>(), Arg.Is("Something went wrong while creating assignment"));
  }

  [Fact]
  public async Task CreateAssignment_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _assignmentRepository.AddAsync(Arg.Any<Assignment>())
      .Returns(FakeAssignment());

    // Act
    var result = await _sut.CreateAssignment(FakeAssignment());

    // Assert
    result.Should().BeEquivalentTo(FakeAssignment());
  }

  [Fact]
  public async Task CreateAssignment_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _assignmentRepository.AddAsync(Arg.Any<Assignment>())
      .Returns(FakeAssignment());

    // Act
    await _sut.CreateAssignment(FakeAssignment());

    // Assert
    _logger.Received(1).LogInformation("Creating assignment");
    _logger.Received(1).LogInformation("Assignment created in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task UpdateAssignment_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Not found");
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.UpdateAssignment(FakeAssignment());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while updating assignment (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task UpdateAssignment_ShouldReturnUpdatedObject_WhenSucceeded()
  {
    // Arrange
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Returns(FakeAssignment());
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    var result = await _sut.UpdateAssignment(FakeAssignment());

    // Assert
    result.Should().BeEquivalentTo(FakeAssignment());
  }

  [Fact]
  public async Task UpdateAssignment_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Returns(FakeAssignment());
    _projectRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Project2>>())
      .Returns(FakeProject());

    // Act
    await _sut.UpdateAssignment(FakeAssignment());

    // Assert
    _logger.Received(1).LogInformation("Updating assignment (id: {0})", 1);
    _logger.Received(1).LogInformation("Assignment (id: {0}) updated in {1}ms", 1, Arg.Any<long>());
  }
  #endregion

  #region Patch
  [Fact]
  public async Task MoveAssignment_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.MoveAssignmentToStage(1,1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>()
      .WithMessage("Something went wrong");
    _logger.Received(1)
      .LogError(
      Arg.Is(exception),
      Arg.Is("Something went wrong while moving assignment (id: {0}) to stage (id: {1})"), 
      Arg.Is(1), Arg.Is(1));
  }

  [Fact]
  public async Task MoveAssignment_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var assignment = FakeAssignment();
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Returns(assignment);

    // Act
    var result = await _sut.MoveAssignmentToStage(assignment.Id, assignment.AssignmentStageId);

    // Assert
    result.Should().BeEquivalentTo(assignment);
  }

  [Fact]
  public async Task MoveAssignment_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var assignment = FakeAssignment();
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Returns(assignment);

    // Act
    await _sut.MoveAssignmentToStage(assignment.Id, assignment.AssignmentStageId);

    // Assert
    _logger.Received(1)
      .LogInformation(
        Arg.Is("Moving assignment (id: {0}) to stage (id: {1})"), 
        Arg.Is(assignment.Id), 
        Arg.Is(assignment.AssignmentStageId));
    _logger.Received(1)
      .LogInformation(
        Arg.Is("Assignment (id: {0}) moved to stage (id: {1}) in {2}ms"), 
        Arg.Is(assignment.Id), 
        Arg.Is(assignment.AssignmentStageId), 
        Arg.Any<long>());
  }

  [Fact]
  public async Task SignUpToAssignment_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.SignUpUserToAssignment(1, 1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.LogError(
      Arg.Is(exception),
      Arg.Is("Something went wrong while signing up user(id: {0}) to assignment(id: {1})"),
      Arg.Is(1), Arg.Is(1));
  }

  [Fact]
  public async Task SignUpToAssignment_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var assignment = FakeAssignment();
    var user = FakeUser();
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Returns(assignment);
    _userRepository.FirstOrDefaultAsync(Arg.Any<Specification<User>>())
      .Returns(user);

    // Act
    var result = await _sut.SignUpUserToAssignment(assignment.Id, user.Id);

    // Assert
    result.Should().BeEquivalentTo(assignment);
  }

  [Fact]
  public async Task SignUpToAssignment_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var assignment = FakeAssignment();
    var user = FakeUser();
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<Specification<Assignment>>())
      .Returns(assignment);
    _userRepository.FirstOrDefaultAsync(Arg.Any<Specification<User>>())
      .Returns(user);

    // Act
    await _sut.SignUpUserToAssignment(assignment.Id, user.Id);

    // Assert
    _logger.Received(1)
      .LogInformation(
        Arg.Is("Signing up user (id: {0}) to assignment (id: {1})"),
        Arg.Is(user.Id), Arg.Is(assignment.Id));
    _logger.Received(1)
      .LogInformation(
        Arg.Is("User (id: {0}) signed up to assignment (id: {1}) in {3}ms"),
        Arg.Is(user.Id), Arg.Is(assignment.Id), Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteAssignment_ShouldLogMessagesAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Not found");
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteAssignment(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while deleting assignment (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task DeleteAssignment_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Returns(FakeAssignment());

    // Act
    var result = await _sut.DeleteAssignment(1);

    // Assert
    result.Should().BeEquivalentTo(FakeAssignment());
  }

  [Fact]
  public async Task DeleteAssignment_ShouldLogMessages_WhenInvoked()
  {
    _assignmentRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Assignment>>())
      .Returns(FakeAssignment());

    // Act
    await _sut.DeleteAssignment(1);

    // Assert
    _logger.Received(1).LogInformation("Deleting assignment (id: {0})", 1);
    _logger.Received(1).LogInformation("Assignment (id: {0}) deleted in {1}ms", 1, Arg.Any<long>());
  }
  #endregion
}
