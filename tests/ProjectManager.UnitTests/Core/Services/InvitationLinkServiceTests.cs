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
public class InvitationLinkServiceTests
{
  private readonly InvitationLinkService _sut;
  private readonly IRepository<InvitationLink> _invitationRepository = Substitute.For<IRepository<InvitationLink>>();
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly ILoggerAdapter<InvitationLinkService> _logger = Substitute.For<ILoggerAdapter<InvitationLinkService>>();

  public InvitationLinkServiceTests()
  {
    _sut = new InvitationLinkService(_invitationRepository, _projectRepository, _logger);
  }

  #region Create
  [Fact]
  public async Task GenerateInvitationLink_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _invitationRepository.AddAsync(Arg.Any<InvitationLink>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.GenerateInvitationLink(FakeInvitationLink());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while generating invitation link for project: {0}"), Arg.Is(1));
  }

  [Fact]
  public async Task GenerateInvitationLink_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var invitationToCreate = FakeInvitationLink();
    _invitationRepository.AddAsync(Arg.Any<InvitationLink>())
      .Returns(invitationToCreate);

    // Act
    var result = await _sut.GenerateInvitationLink(invitationToCreate);

    // Assert
    result.Should().BeEquivalentTo(invitationToCreate);
  }

  [Fact]
  public async Task GenerateInvitationLink_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var invitationToCreate = FakeInvitationLink();
    _invitationRepository.AddAsync(Arg.Any<InvitationLink>())
      .Returns(invitationToCreate);

    // Act
    await _sut.GenerateInvitationLink(invitationToCreate);

    // Assert
    _logger.Received(1).LogInformation("Generating invitation link for project: {0}", Arg.Is(invitationToCreate.ProjectId));
    _logger.Received(1).LogInformation("Invitation link for project {0} generated in {1}ms", Arg.Is(invitationToCreate.ProjectId), Arg.Any<long>());
  }
  #endregion

  #region Retrieve
  [Fact]
  public async Task GetInvitationLink_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.GetInvitationLink("test");

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving invitation link (url: {0})"), Arg.Is("test"));
  }

  [Fact]
  public async Task GetInvitationLink_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var invitationToRetrieve = FakeInvitationLink();
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Returns(invitationToRetrieve);

    // Act
    var result = await _sut.GetInvitationLink(invitationToRetrieve.Url);

    // Assert
    result.Should().BeEquivalentTo(invitationToRetrieve);
  }

  [Fact]
  public async Task GetInvitationLink_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var invitationToRetrieve = FakeInvitationLink();
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Returns(invitationToRetrieve);

    // Act
    await _sut.GetInvitationLink(invitationToRetrieve.Url);

    // Assert
    _logger.Received(1).LogInformation("Retrieving invitation link (url: {0})", Arg.Is(invitationToRetrieve.Url));
    _logger.Received(1).LogInformation("Invitation link (url: {0}) retrieved in {1}ms", Arg.Is(invitationToRetrieve.Url), Arg.Any<long>());
  }
  #endregion

  #region Patch
  [Fact]
  public async Task SetInvitationLinkAsUsed_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.SetInvitationLinkAsUsed(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while setting invitation link {0} as used"), Arg.Is(1));
  }

  [Fact]
  public async Task SetInvitationLinkAsUsed_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var invitationToSet = FakeInvitationLink();
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Returns(invitationToSet);

    // Act
    var result = await _sut.SetInvitationLinkAsUsed(invitationToSet.Id);

    // Assert
    result.Should().BeEquivalentTo(invitationToSet);
    result.IsUsed.Should().Be(true);
  }

  [Fact]
  public async Task SetInvitationAsUsed_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var invitationToSet = FakeInvitationLink();
    _invitationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<InvitationLink>>())
      .Returns(invitationToSet);

    // Act
    await _sut.SetInvitationLinkAsUsed(invitationToSet.Id);

    // Assert
    _logger.Received(1).LogInformation("Setting invitation link (id : {0}) as used", Arg.Is(invitationToSet.Id));
    _logger.Received(1).LogInformation("Invitation link {0} set as used in {1}ms", Arg.Is(invitationToSet.Id), Arg.Any<long>());
  }
  #endregion
}
