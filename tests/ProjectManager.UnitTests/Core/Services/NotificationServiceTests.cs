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
public class NotificationServiceTests
{
  private readonly NotificationService _sut;
  private readonly IRepository<Notification> _notificationRepository = Substitute.For<IRepository<Notification>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly ILoggerAdapter<NotificationService> _logger = Substitute.For<ILoggerAdapter<NotificationService>>();

  public NotificationServiceTests()
  {
    _sut = new NotificationService(_notificationRepository, _userRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllNotifications_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _notificationRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllNotifications();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving notifications"));
  }

  [Fact]
  public async Task RetrieveAllNotifications_ShouldReturnEmptyList_WhenNoNotificationsExist()
  {
    // Arrange
    _notificationRepository.ListAsync()
      .Returns(new List<Notification>());

    // Act
    var result = await _sut.RetrieveAllNotifications();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllNotifications_ShouldReturnNotifications_WhenSomeNotificationsExist()
  {
    // Arrange
    var expectedNotifications = FakeNotificationsList();
    _notificationRepository.ListAsync()
      .Returns(expectedNotifications);

    // Act
    var result = await _sut.RetrieveAllNotifications();

    // Assert
    result.Should().BeEquivalentTo(expectedNotifications);
  }

  [Fact]
  public async Task RetrieveAllNotifications_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _notificationRepository.ListAsync()
      .Returns(FakeNotificationsList());

    // Act
    await _sut.RetrieveAllNotifications();

    // Assert
    _logger.Received(1).LogInformation("Retrieving all notifications");
    _logger.Received(1).LogInformation("Notifications retrieved in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Create
  [Fact]
  public async Task CreateNotification_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _notificationRepository.AddAsync(Arg.Any<Notification>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateNotification(FakeNotification());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while creating notification"));
  }

  [Fact]
  public async Task CreateNotification_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedNotification = FakeNotification();
    _notificationRepository.AddAsync(Arg.Any<Notification>())
      .Returns(expectedNotification);

    // Act
    var result = await _sut.CreateNotification(expectedNotification);

    // Assert
    result.Should().BeEquivalentTo(expectedNotification);
  }

  [Fact]
  public async Task CreateNotification_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _notificationRepository.AddAsync(Arg.Any<Notification>())
      .Returns(FakeNotification());

    // Act
    await _sut.CreateNotification(FakeNotification());

    // Assert
    _logger.Received(1).LogInformation("Creating notification");
    _logger.Received(1).LogInformation("Notification created in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Patch
  [Fact]
  public async Task SetNotificationAsSeen_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _notificationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Notification>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.SetNotificationAsSeen(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while setting notification (id {0}) as seen"), Arg.Is(1));
  }

  [Fact]
  public async Task SetNotificationAsSeen_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedNotification = FakeNotification();
    _notificationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Notification>>())
      .Returns(expectedNotification);

    // Act
    var result = await _sut.SetNotificationAsSeen(expectedNotification.Id);

    // Assert
    expectedNotification.IsSeen = true;
    result.Should().BeEquivalentTo(expectedNotification);
  }

  [Fact]
  public async Task SetNotificationAsSeen_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var expectedNotification = FakeNotification();
    _notificationRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Notification>>())
      .Returns(expectedNotification);

    // Act
    await _sut.SetNotificationAsSeen(expectedNotification.Id);

    // Assert
    _logger.Received(1).LogInformation("Setting notification (id: {0}) as seen", Arg.Is(expectedNotification.Id));
    _logger.Received(1).LogInformation("Notification (id: {0}) set as seen in {1}ms", Arg.Is(expectedNotification.Id), Arg.Any<long>());
  }
  #endregion
}
