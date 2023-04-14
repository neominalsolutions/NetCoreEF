using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Data.Identity;
using System.Security.Claims;

namespace NetCoreEF.Areas.Admin.Controllers
{
  public class RoleController : AdminBaseController
  {
    private readonly RoleManager<ApplicationRole> roleManager;

    public RoleController(RoleManager<ApplicationRole> roleManager, IMediator mediator):base(mediator)
    {
      this.roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
      var roles = await roleManager.Roles.ToListAsync();

      return View();
    }


    public async Task<IActionResult> Create()
    {
      var role = new ApplicationRole();
      role.Name = "Admin";
      role.Description = "ADMIN";

      var result = await this.roleManager.CreateAsync(role);
      await this.roleManager.AddClaimAsync(role,new Claim("ReportViewer", "ReadWriteAccess"));

      if (result.Succeeded)
      {
        ViewBag.Message = "Role oluştu";
      }

      return View();
    }
  }
}
