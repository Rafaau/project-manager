using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.SharedKernel;
public static class ApiHelper
{
  public static ObjectResult ReturnErrorResult(this ControllerBase controller, Exception error)
  {
    return controller.StatusCode(StatusCodes.Status500InternalServerError, error.AsResponse<Exception>());
  }
}
