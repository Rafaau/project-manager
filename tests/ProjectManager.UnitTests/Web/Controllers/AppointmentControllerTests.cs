using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.Api;
using ProjectManager.Web.MappingProfiles;
using Xunit;
using static ProjectManager.UnitTests.ODataQueryOptionsFactory;
using static ProjectManager.UnitTests.FakesFactory;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Microsoft.AspNetCore.Http;

namespace ProjectManager.UnitTests.Web.Controllers;
public class AppointmentControllerTests
{
  private readonly AppointmentController _sut;
  private readonly IAppointmentService _appointmentService = Substitute.For<IAppointmentService>();
  private IMapper _mapper;

  public AppointmentControllerTests()
  {
    var config = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });
    _mapper = config.CreateMapper();

    _sut = new AppointmentController(_appointmentService, _mapper);
  }

  [Fact]
  public async Task List_ShouldReturnEmptyList_WhenNoAppointmentsExist()
  {
    // Arrange
    _appointmentService.RetrieveAllAppointments()
      .Returns(Enumerable.Empty<Appointment>().AsQueryable());

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<Appointment>());
    var resultData = (Response<AppointmentComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEmpty();
  }

  [Fact]
  public async Task List_ShouldReturnAppointments_WhenSomeAppointmentsExist()
  {
    // Arrange
    var appointments = FakeAppointmentsList();
    _appointmentService.RetrieveAllAppointments()
      .Returns(appointments.AsQueryable());

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<Appointment>());
    var resultData = (Response<AppointmentComplex[]>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(200);
    resultData.Data.Should().BeEquivalentTo(_mapper.Map<AppointmentComplex[]>(appointments));
  }

  [Fact]
  public async Task List_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _appointmentService.RetrieveAllAppointments()
      .Throws<Exception>();

    // Act
    var result = (ObjectResult) await _sut.List(GetQueryOptions<Appointment>());

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }

  [Fact]
  public async Task Post_ShouldReturn201AndObject_WhenSucceeded()
  {
    // Arrange
    var appointmentToAdd = _mapper.Map<AppointmentRequest>(FakeAppointment());
    _appointmentService.CreateAppointment(Arg.Any<Appointment>())
      .Returns(FakeAppointment());

    // Act
    var result = (ObjectResult) await _sut.Post(appointmentToAdd);
    var resultData = (Response<AppointmentRequest>) result.Value!;

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status201Created);
    resultData.Data.Should().BeEquivalentTo(appointmentToAdd);
  }

  [Fact]
  public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
  {
    // Arrange
    _appointmentService.CreateAppointment(Arg.Any<Appointment>())
      .Throws<Exception>();

    // Act
    var result = (ObjectResult)await _sut.Post(_mapper.Map<AppointmentRequest>(FakeAppointment()));

    // Assert
    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
  }
}
