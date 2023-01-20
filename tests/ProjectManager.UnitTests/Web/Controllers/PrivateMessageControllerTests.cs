using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
public class PrivateMessageControllerTests
{
  private readonly PrivateMessageController _sut;
  private readonly IPrivateMessageService _messageService = Substitute.For<IPrivateMessageService>();
  private readonly IMapper _mapper;

  public PrivateMessageControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new PrivateMessageController(_messageService, _mapper);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    _messageService.RetrieveAllMessages()
      .Returns(Enumerable.Empty<PrivateMessage>().AsQueryable());

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<PrivateMessage>());
    var resultData = (Response<PrivateMessageComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnOkAndMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var expected = _mapper.Map<PrivateMessageComplex[]>(FakePrivateMessagesList());
    _messageService.RetrieveAllMessages()
      .Returns(FakePrivateMessagesList().AsQueryable());

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<PrivateMessage>());
    var resultData = (Response<PrivateMessageComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.RetrieveAllMessages()
      .Throws<Exception>();

    // Act
    var result = (ObjectResult)await _sut.List(GetQueryOptions<PrivateMessage>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task GetUserConversations_ShouldReturnEmptyList_WhenNoConversationsExist()
  {
    // Arrange
    _messageService.GetUserConversations(Arg.Any<int>())
      .Returns(Enumerable.Empty<PrivateMessage>().ToArray());

    // Act
    var result = (ObjectResult) await _sut.GetUserConversations(1);
    var resultData = (Response<PrivateMessageComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEmpty();
  }

  [Fact]
  public async Task GetUserConversations_ShouldReturnOkAndMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var expected = _mapper.Map<PrivateMessageComplex[]>(FakePrivateMessagesList());
    _messageService.GetUserConversations(Arg.Any<int>())
      .Returns(FakePrivateMessagesList().ToArray());

    // Act
    var result = (ObjectResult) await _sut.GetUserConversations(1);
    var resultData = (Response<PrivateMessageComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task GetUserConversations_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.GetUserConversations(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.GetUserConversations(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnCreatedAndObject_WhenSucceeded()
  {
    // Arrange
    var messageToCreate = _mapper.Map<PrivateMessageRequest>(FakePrivateMessage());
    var expected = _mapper.Map<PrivateMessageComplex>(FakePrivateMessage());
    _messageService.PostMessage(Arg.Any<PrivateMessage>())
      .Returns(FakePrivateMessage());

    // Act
    var result = (ObjectResult) await _sut.Post(messageToCreate);
    var resultData = (Response<PrivateMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.PostMessage(Arg.Any<PrivateMessage>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<PrivateMessageRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task SetAsSeen_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<PrivateMessageComplex>(FakePrivateMessage());
    _messageService.SetMessageAsSeen(Arg.Any<int>())
      .Returns(FakePrivateMessage());

    // Act
    var result = (ObjectResult) await _sut.SetAsSeen(expected.Id);
    var resultData = (Response<PrivateMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task SetAsSeen_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.SetMessageAsSeen(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.SetAsSeen(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var messageToUpdate = _mapper.Map<PrivateMessageSimplified>(FakePrivateMessage());
    var expected = _mapper.Map<PrivateMessageComplex>(FakePrivateMessage());
    _messageService.EditMessage(Arg.Any<PrivateMessage>())
      .Returns(FakePrivateMessage());

    // Act
    var result = (ObjectResult) await _sut.Update(messageToUpdate);
    var resultData = (Response<PrivateMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.EditMessage(Arg.Any<PrivateMessage>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Update(Arg.Any<PrivateMessageSimplified>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<PrivateMessageComplex>(FakePrivateMessage());
    _messageService.DeleteMessage(Arg.Any<int>())
      .Returns(FakePrivateMessage());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<PrivateMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.PostDate));
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.DeleteMessage(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
