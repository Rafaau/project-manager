using System.Diagnostics;
using ProjectManager.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Web.Controllers;
public class HomeController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
