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
public class MessageControllerTests
{
  private readonly MessageController _sut;
  private readonly IMessageService _messageService = Substitute.For<IMessageService>();
  private readonly IMapper _mapper;

  public MessageControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new MessageController(_messageService, _mapper);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    _messageService.RetrieveAllMessages().Returns(Enumerable.Empty<Message>().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Message>());
    var resultCast = (OkObjectResult) result;
    var resultData = (Response<MessageComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<MessageComplex[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnMessagesResponse_WhenMessagesExist()
  {
    // Arrange
    var messagesResponse = _mapper.Map<MessageComplex[]>(FakeMessageList());
    _messageService.RetrieveAllMessages().Returns(FakeMessageList().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Message>());
    var resultCast = (ObjectResult) result;
    var resultData = (Response<MessageComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<MessageComplex[]>().Should().BeEquivalentTo(messagesResponse);
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.RetrieveAllMessages().Throws<Exception>();

    // Act
    var result = await _sut.List(GetQueryOptions<Message>());
    var resultCast = (ObjectResult) result;

    // Assert
    resultCast.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    _messageService.PostMessage(Arg.Any<Message>())
      .Returns(FakeMessage());
    var messageResponse = _mapper.Map<MessageRequest>(FakeMessage());

    // Act
    var result = (OkObjectResult) await _sut.Post(messageResponse);
    var resultData = (Response<MessageRequest>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(messageResponse);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.PostMessage(Arg.Any<Message>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<MessageRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var messageResponse = _mapper.Map<MessageComplex>(FakeMessage());
    _messageService.EditMessage(Arg.Any<Message>())
      .Returns(FakeMessage());

    // Act
    var result = (ObjectResult) await _sut.Patch(messageResponse);
    var resultData = (Response<MessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(messageResponse);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _messageService.EditMessage(Arg.Any<Message>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Patch(Arg.Any<MessageComplex>());

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
    var resultData = (Response<MessageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(_mapper.Map<MessageComplex>(FakeMessage()));
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
