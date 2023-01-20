using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class AppointmentController : BaseApiController
{
  private readonly IAppointmentService _appointmentService;
  private readonly IMapper _mapper;

  public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
  {
    _appointmentService = appointmentService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<Appointment> queryOptions)
  {
    try
    {
      var retrievedAppointments =
        queryOptions.ApplyTo(await _appointmentService.RetrieveAllAppointments());

      return Ok(_mapper.Map<AppointmentComplex[]>(retrievedAppointments).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post(AppointmentRequest request)
  {
    try
    {
      var mapped = _mapper.Map<Appointment>(request);
      var createdAppointment = await _appointmentService.CreateAppointment(mapped);

      return CreatedAtAction(null, _mapper.Map<AppointmentRequest>(createdAppointment).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
