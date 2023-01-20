using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ProjectManager.UnitTests.Web.Controllers;
public class ChatChannelControllerTests
{
  private readonly ChatChannelController _sut;
  private readonly IChatChannelService _channelService = Substitute.For<IChatChannelService>();
  private readonly IMapper _mapper;

  public ChatChannelControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new ChatChannelController(_channelService, _mapper);
  }

  [Fact]
  public async Task Create_ShouldReturnCreatedAndObject_WhenRequestIsValid()
  {
    // Arrange
    _channelService.CreateChatChannel(Arg.Any<ChatChannel>())
      .Returns(FakeChatChannel());
    var channelResponse = _mapper.Map<ChatChannelRequest>(FakeChatChannel());

    // Act
    var result = (ObjectResult) await _sut.Create(channelResponse);
    var resultData = (Response<ChatChannelComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.Data.Should().BeEquivalentTo(channelResponse);
  }

  [Fact]
  public async Task Create_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _channelService.CreateChatChannel(Arg.Any<ChatChannel>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Create(Arg.Any<ChatChannelRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var channelToUpdate = _mapper.Map<ChatChannelSimplified>(FakeChatChannel());
    _channelService.UpdateChatChannel(Arg.Any<ChatChannel>())
      .Returns(FakeChatChannel());

    // Act
    var result = (ObjectResult) await _sut.Update(channelToUpdate);
    var resultData = (Response<ChatChannelComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(channelToUpdate);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _channelService.UpdateChatChannel(Arg.Any<ChatChannel>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Update(Arg.Any<ChatChannelSimplified>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<ChatChannelComplex>(FakeChatChannel());
    _channelService.DeleteChatChannel(Arg.Any<int>())
      .Returns(FakeChatChannel());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<ChatChannelComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _channelService.DeleteChatChannel(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
