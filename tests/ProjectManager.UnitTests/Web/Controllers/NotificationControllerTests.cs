using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.Api;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.MappingProfiles;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;
using static ProjectManager.UnitTests.ODataQueryOptionsFactory;

namespace ProjectManager.UnitTests.Web.Controllers;
public class NotificationControllerTests
{
  private readonly NotificationController _sut;
  private readonly INotificationService _notificationService = Substitute.For<INotificationService>();
  private readonly IMapper _mapper;

  public NotificationControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new NotificationController(_notificationService, _mapper);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoNotificationsExist()
  {
    // Arrange
    _notificationService.RetrieveAllNotifications()
      .Returns(Enumerable.Empty<Notification>().AsQueryable());

    // Act
    var result = (ObjectResult)await _sut.List(GetQueryOptions<Notification>());
    var resultData = (Response<NotificationComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<NotificationComplex[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnNotifications_WhenNotificationsExist()
  {
    // Arrange
    var expected = _mapper.Map<NotificationComplex[]>(FakeNotificationsList());
    _notificationService.RetrieveAllNotifications()
      .Returns(FakeNotificationsList().AsQueryable());

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<Notification>());
    var resultData = (Response<NotificationComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.Date));
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _notificationService.RetrieveAllNotifications()
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<Notification>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnCreatedAndObject_WhenSucceeded()
  {
    // Arrange
    var notificationToCreate = _mapper.Map<NotificationRequest>(FakeNotification());
    var expected = _mapper.Map<NotificationComplex>(FakeNotification());
    _notificationService.CreateNotification(Arg.Any<Notification>())
      .Returns(FakeNotification());

    // Act
    var result = (ObjectResult) await _sut.Post(notificationToCreate);
    var resultData = (Response<NotificationComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.Date));
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _notificationService.CreateNotification(Arg.Any<Notification>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<NotificationRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task SetAsSeen_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<NotificationComplex>(FakeNotification());
    _notificationService.SetNotificationAsSeen(Arg.Any<int>())
      .Returns(FakeNotification());

    // Act
    var result = (ObjectResult) await _sut.SetAsSeen(expected.Id);
    var resultData = (Response<NotificationComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.Date));
  }

  [Fact]
  public async Task SetAsSeen_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _notificationService.SetNotificationAsSeen(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.SetAsSeen(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
