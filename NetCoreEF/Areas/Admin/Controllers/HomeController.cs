using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreEF.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class HomeController : AdminBaseController
  {

    public HomeController(IMediator mediator):base(mediator)
    {

    }

    public IActionResult Index()
    {
      return View();
    }
  }
}
