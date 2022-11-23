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
public class UserControllerTests
{
  private readonly UserController _sut;
  private readonly IUserService _userService = Substitute.For<IUserService>();
  private readonly IMapper _mapper;

  public UserControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new UserController(_mapper, _userService);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoUsersExist()
  {
    // Arrange
    _userService.RetrieveAllUsers()
      .Returns(Enumerable.Empty<User>().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<User>());
    var resultCast = (OkObjectResult) result;
    var resultData = (Response<UserComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<UserComplex[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnUsersResponse_WhenUsersExist()
  {
    // Arrange
    var usersResponse = _mapper.Map<UserComplex[]>(FakeUsersList());
    _userService.RetrieveAllUsers()
      .Returns(FakeUsersList().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<User>());
    var resultCast = (ObjectResult) result;
    var resultData = (Response<UserComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<UserComplex[]>().Should().BeEquivalentTo(usersResponse);
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _userService.RetrieveAllUsers()
      .Throws<Exception>();

    // Act
    var result = await _sut.List(GetQueryOptions<User>());
    var resultCast = (ObjectResult) result;

    // Assert
    resultCast.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task GetByEmail_ShouldReturnOkAndResponse_WhenUserExist()
  {
    // Arrange
    var userResponse = _mapper.Map<UserComplex>(FakeUser());
    _userService.RetrieveUserByEmail(Arg.Any<string>())
      .Returns(FakeUser());

    // Act
    var result = (ObjectResult) await _sut.GetByEmail("");
    var resultData = (Response<UserComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<UserComplex>().Should().BeEquivalentTo(userResponse);
  }

  [Fact]
  public async Task GetByEmail_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _userService.RetrieveUserByEmail(Arg.Any<string>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.GetByEmail("");

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndResponse_WhenSucceeded()
  {
    // Arrange
    var userResponse = _mapper.Map<UserRequest>(FakeUser());
    _userService.CreateUser(Arg.Any<User>())
      .Returns(FakeUser());

    // Act
    var result = (CreatedAtActionResult) await _sut.Post(userResponse);
    var resultData = (UserSimplified) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.As<UserSimplified>().Should().BeEquivalentTo(userResponse);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _userService.CreateUser(Arg.Any<User>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(_mapper.Map<UserRequest>(FakeUser()));

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndResponse_WhenSucceeded()
  {
    // Arrange
    var userResponse = _mapper.Map<UserSimplified>(FakeUser());
    _userService.UpdateUser(Arg.Any<User>())
      .Returns(FakeUser());

    // Act
    var result = (ObjectResult) await _sut.Update(userResponse);
    var resultData = (Response<UserSimplified>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<UserSimplified>().Should().BeEquivalentTo(userResponse);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _userService.UpdateUser(Arg.Any<User>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Update(_mapper.Map<UserSimplified>(FakeUser()));

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndResponse_WhenSucceeded()
  {
    // Arrange
    var userResponse = _mapper.Map<UserComplex>(FakeUser());
    _userService.DeleteUser(Arg.Any<int>())
      .Returns(FakeUser());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<UserComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<UserComplex>().Should().BeEquivalentTo(userResponse);
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _userService.DeleteUser(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
