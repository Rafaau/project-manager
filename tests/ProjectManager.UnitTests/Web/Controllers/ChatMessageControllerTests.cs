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
public class ChatMessageControllerTests
{
  private readonly ChatMessageController _sut;
  private readonly IChatMessageService _messageService = Substitute.For<IChatMessageService>();
  private readonly IMapper _mapper;

  public ChatMessageControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new ChatMessageController(_messageService, _mapper);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    _messageService.RetrieveAllMessages().Returns(Enumerable.Empty<ChatMessage>().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<ChatMessage>());
    var resultCast = (OkObjectResult) result;
    var resultData = (Response<ChatMessageComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<ChatMessageComplex[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnMessagesResponse_WhenMessagesExist()
  {
    // Arrange
    var messagesResponse = _mapper.Map<ChatMessageComplex[]>(FakeMessageList());
    _messageService.RetrieveAllMessages().Returns(FakeMessageList().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<ChatMessage>());
    var resultCast = (ObjectResult) result;
    var resultData = (Response<ChatMessageComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<ChatMessageComplex[]>().Should().BeEquivalentTo(messagesResponse);
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.RetrieveAllMessages().Throws<Exception>();

    // Act
    var result = await _sut.List(GetQueryOptions<ChatMessage>());
    var resultCast = (ObjectResult) result;

    // Assert
    resultCast.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    _messageService.PostMessage(Arg.Any<ChatMessage>())
      .Returns(FakeMessage());
    var messageResponse = _mapper.Map<ChatMessageRequest>(FakeMessage());

    // Act
    var result = (OkObjectResult) await _sut.Post(messageResponse);
    var resultData = (Response<ChatMessageRequest>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(messageResponse);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.PostMessage(Arg.Any<ChatMessage>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<ChatMessageRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var messageResponse = _mapper.Map<ChatMessageComplex>(FakeMessage());
    _messageService.EditMessage(Arg.Any<int>(), Arg.Any<string>())
      .Returns(FakeMessage());

    // Act
    var result = (ObjectResult) await _sut.Patch(messageResponse.Id, messageResponse.Content);
    var resultData = (Response<ChatMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(messageResponse);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.EditMessage(Arg.Any<int>(), Arg.Any<string>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Patch(1, "");

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    _messageService.DeleteMessage(Arg.Any<int>())
      .Returns(FakeMessage());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<ChatMessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(_mapper.Map<ChatMessageComplex>(FakeMessage()));
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
