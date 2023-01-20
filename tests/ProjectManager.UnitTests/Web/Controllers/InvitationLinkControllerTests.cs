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

namespace ProjectManager.UnitTests.Web.Controllers;
public class InvitationLinkControllerTests
{
  private readonly InvitationLinkController _sut;
  private readonly IInvitationLinkService _invitationService = Substitute.For<IInvitationLinkService>();
  private readonly IMapper _mapper;

  public InvitationLinkControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new InvitationLinkController(_invitationService, _mapper);
  }

  [Fact]
  public async Task Post_ShouldReturnCreatedAndObject_WhenRequestIsValid()
  {
    // Arrange
    _invitationService.GenerateInvitationLink(Arg.Any<InvitationLink>())
      .Returns(FakeInvitationLink());
    var request = _mapper.Map<InvitationLinkRequest>(FakeInvitationLink());

    // Act
    var result = (ObjectResult) await _sut.Post(request);
    var resultData = (Response<InvitationLinkComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.Data.Should().BeEquivalentTo(request);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _invitationService.GenerateInvitationLink(Arg.Any<InvitationLink>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<InvitationLinkRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Get_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<InvitationLinkComplex>(FakeInvitationLink());
    _invitationService.GetInvitationLink(Arg.Any<string>())
      .Returns(FakeInvitationLink());

    // Act
    var result = (ObjectResult) await _sut.Get(expected.Url);
    var resultData = (Response<InvitationLinkComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.ExpirationTime));
  }

  [Fact]
  public async Task Get_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _invitationService.GetInvitationLink(Arg.Any<string>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Get("");

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Patch_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var expected = _mapper.Map<InvitationLinkComplex>(FakeInvitationLink());
    _invitationService.SetInvitationLinkAsUsed(Arg.Any<int>())
      .Returns(FakeInvitationLink());

    // Act
    var result = (ObjectResult) await _sut.Patch(expected.Id);
    var resultData = (Response<InvitationLinkComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(expected, opt => opt.Excluding(e => e.ExpirationTime));
  }

  [Fact]
  public async Task Patch_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _invitationService.SetInvitationLinkAsUsed(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Patch(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
