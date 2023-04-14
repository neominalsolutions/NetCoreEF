using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreEF.Areas.Admin.Controllers
{

  [Authorize(Roles ="Admin")]
  public class AdminBaseController : Controller
  {
    protected IMediator mediator;

    public AdminBaseController(IMediator mediator)
    {
      this.mediator = mediator;
    }
   
  }
}
