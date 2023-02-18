using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Web.Api;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : Controller
{
}
