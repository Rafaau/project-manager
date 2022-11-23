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
public class ProjectControllerTests
{
  private readonly ProjectController _sut;
  private readonly IProjectService _projectService = Substitute.For<IProjectService>();
  private IMapper _mapper;

  public ProjectControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new ProjectController(_mapper, _projectService);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoProjectsExist()
  {
    // Arrange
    _projectService.RetrieveAllProjects()
      .Returns(Enumerable.Empty<Project2>().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Project2>());
    var resultCast = (OkObjectResult) result;
    var resultData = (Response<ProjectSimplified[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<ProjectSimplified[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnProjectsResponse_WhenProjectsExist()
  {
    // Arrange
    var projectsResponse = _mapper.Map<ProjectSimplified[]>(FakeProjectsList());
    _projectService.RetrieveAllProjects()
      .Returns(FakeProjectsList().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Project2>());
    var resultCast = (ObjectResult) result;
    var resultData = (Response<ProjectSimplified[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<ProjectSimplified[]>().Should().BeEquivalentTo(projectsResponse);
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _projectService.RetrieveAllProjects()
      .Throws<Exception>();

    // Act
    var result = await _sut.List(GetQueryOptions<Project2>());
    var resultCast = (ObjectResult) result;

    // Assert
    resultCast.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task GetById_ShouldReturnOkAndResponse_WhenProjectExist()
  {
    // Arrange
    _projectService.RetrieveProjectById(Arg.Any<int>())
      .Returns(FakeProject());

    // Act
    var result = (ObjectResult) await _sut.GetById(1);
    var resultData = (Response<ProjectComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(_mapper.Map<ProjectComplex>(FakeProject()));
  }

  [Fact]
  public async Task GetById_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _projectService.RetrieveProjectById(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.GetById(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndResponse_WhenSucceeded()
  {
    // Arrange
    var projectResponse = _mapper.Map<ProjectRequest>(FakeProject());
    _projectService.CreateProject(Arg.Any<Project2>())
      .Returns(FakeProject());

    // Act
    var result = (OkObjectResult) await _sut.Post(projectResponse);
    var resultData = (Response<ProjectRequest>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(projectResponse);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _projectService.CreateProject(Arg.Any<Project2>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(_mapper.Map<ProjectRequest>(FakeProject()));

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndResponse_WhenSucceeded()
  {
    // Arrange
    var projectResponse = _mapper.Map<ProjectComplex>(FakeProject());
    _projectService.UpdateProject(Arg.Any<Project2>())
      .Returns(FakeProject());

    // Act
    var result = (ObjectResult) await _sut.Update(projectResponse);
    var resultData = (Response<ProjectComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<ProjectComplex>().Should().BeEquivalentTo(projectResponse);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _projectService.UpdateProject(Arg.Any<Project2>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Update(Arg.Any<ProjectComplex>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    _projectService.DeleteProject(Arg.Any<int>())
      .Returns(FakeProject());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<ProjectComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.As<ProjectComplex>().Should().BeEquivalentTo(_mapper.Map<ProjectComplex>(FakeProject()));
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _projectService.DeleteProject(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
