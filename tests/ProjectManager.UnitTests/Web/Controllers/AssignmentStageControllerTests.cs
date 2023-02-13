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
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel;
using ProjectManager.Web.Api;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.MappingProfiles;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Web.Controllers;
public class AssignmentStageControllerTests
{
  private readonly AssignmentStageController _sut;
  private readonly IAssignmentStageService _stageService = Substitute.For<IAssignmentStageService>();
  private readonly IMapper _mapper;

  public AssignmentStageControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new AssignmentStageController(_stageService, _mapper);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var stageToAdd = _mapper.Map<AssignmentStageRequest>(FakeStage());
    _stageService.AddAssignmentStage(Arg.Any<AssignmentStage>())
      .Returns(FakeStage());

    // Act
    var result = (ObjectResult) await _sut.Post(stageToAdd);
    var resultData = (Response<AssignmentStageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(201);
    resultData.Data.Should().BeEquivalentTo(stageToAdd, o => o.ExcludingMissingMembers());
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _stageService.AddAssignmentStage(Arg.Any<AssignmentStage>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(_mapper.Map<AssignmentStageRequest>(FakeStage()));

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task EditName_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var stageToPatch = _mapper.Map<AssignmentStageComplex>(FakeStage());
    _stageService.UpdateAssignmentStage(Arg.Any<int>(), Arg.Any<string>())
      .Returns(FakeStage());

    // Act
    var result = (ObjectResult) await _sut.EditAssignmentStageName(stageToPatch.Id, stageToPatch.Name);
    var resultData = (Response<AssignmentStageComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(stageToPatch);
  }

  [Fact]
  public async Task EditName_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _stageService.UpdateAssignmentStage(Arg.Any<int>(), Arg.Any<string>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.EditAssignmentStageName(1, "Test");

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var stageToDelete = _mapper.Map<AssignmentStageComplex>(FakeStage());
    _stageService.DeleteAssignmentStage(Arg.Any<int>())
      .Returns(FakeStage());

    // Act
    var result = (ObjectResult) await _sut.Delete(stageToDelete.Id);
    var resultData = (Response<AssignmentStageSimplified>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(stageToDelete, o => o.ExcludingMissingMembers());
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _stageService.DeleteAssignmentStage(Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
