using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Npgsql;
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
public class AssignmentControllerTests
{
  private readonly AssignmentController _sut;
  private readonly IAssignmentService _assignmentService = Substitute.For<IAssignmentService>();
  private readonly IMapper _mapper;

  public AssignmentControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new AssignmentController(_mapper, _assignmentService);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoAssignmentsExist()
  {
    // Arrange
    _assignmentService.RetrieveAllAssignments().Returns(Enumerable.Empty<Assignment>().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Assignment>());
    var resultCast = (OkObjectResult) result;
    var resultData = (Response<AssignmentComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<AssignmentComplex[]>().Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnAssignmentsResponse_WhenAssignmentsExist()
  {
    // Arrange
    var assignmentsResponse = _mapper.Map<AssignmentComplex[]>(FakeAssignmentsList());
    _assignmentService.RetrieveAllAssignments().Returns(FakeAssignmentsList().AsQueryable());

    // Act
    var result = await _sut.List(GetQueryOptions<Assignment>());
    var resultCast = (OkObjectResult)result;
    var resultData = (Response<AssignmentComplex[]>) resultCast.Value!;

    // Assert
    resultCast.StatusCode.Should().Be(200);
    resultData.Data.As<AssignmentComplex[]>().Should().BeEquivalentTo(assignmentsResponse);
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.RetrieveAllAssignments().Throws<Exception>();

    // Act
    var result = await _sut.List(GetQueryOptions<Assignment>());
    var resultCast = (ObjectResult) result;

    // Assert
    resultCast.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturnOkAndObject_WhenRequestIsValid()
  {
    // Arrange
    _assignmentService.CreateAssignment(Arg.Any<Assignment>()).Returns(FakeAssignment());
    var assignmentResponse = _mapper.Map<AssignmentRequest>(FakeAssignment());

    // Act
    var result = (OkObjectResult) await _sut.Post(assignmentResponse);
    var resultData = (Response<AssignmentRequest>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(assignmentResponse);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.CreateAssignment(Arg.Any<Assignment>()).Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Post(Arg.Any<AssignmentRequest>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Update_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var assignmentToUpdate = _mapper.Map<AssignmentComplex>(FakeAssignment());
    _assignmentService.UpdateAssignment(Arg.Any<Assignment>()).Returns(FakeAssignment());

    // Act
    var result = (ObjectResult) await _sut.Update(assignmentToUpdate);
    var resultData = (Response<AssignmentComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(assignmentToUpdate);
  }

  [Fact]
  public async Task Update_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.UpdateAssignment(Arg.Any<Assignment>()).Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Update(Arg.Any<AssignmentComplex>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task MoveToStage_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var assignmentToMove = _mapper.Map<AssignmentComplex>(FakeAssignment());
    _assignmentService.MoveAssignmentToStage(Arg.Any<int>(), Arg.Any<int>())
      .Returns(FakeAssignment());

    // Act
    var result = (ObjectResult) await _sut.MoveToStage(assignmentToMove.Id, 1);
    var resultData = (Response<AssignmentComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(assignmentToMove);
  }

  [Fact]
  public async Task MoveToStage_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.MoveAssignmentToStage(Arg.Any<int>(), Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.MoveToStage(1, 1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task SignUpUser_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    var mapped = _mapper.Map<AssignmentComplex>(FakeAssignment());
    _assignmentService.SignUpUserToAssignment(Arg.Any<int>(), Arg.Any<int>())
      .Returns(FakeAssignment());

    // Act
    var result = (ObjectResult) await _sut.SignUpUserToAssignment(mapped.Id, 1);
    var resultData = (Response<AssignmentComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(mapped);
  }

  [Fact]
  public async Task SignUpUser_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.SignUpUserToAssignment(Arg.Any<int>(), Arg.Any<int>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.SignUpUserToAssignment(1, 1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Delete_ShouldReturnOkAndObject_WhenSucceeded()
  {
    // Arrange
    _assignmentService.DeleteAssignment(Arg.Any<int>()).Returns(FakeAssignment());

    // Act
    var result = (ObjectResult) await _sut.Delete(1);
    var resultData = (Response<AssignmentComplex>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(_mapper.Map<AssignmentComplex>(FakeAssignment()));
  }

  [Fact]
  public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _assignmentService.DeleteAssignment(Arg.Any<int>()).Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.Delete(1);

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
