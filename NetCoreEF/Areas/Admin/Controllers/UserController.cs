using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Data.Identity;
using System.Security.Claims;

namespace NetCoreEF.Areas.Admin.Controllers
{
  public class UserController : Controller
  {
    private readonly UserManager<ApplicationUser> userManager;

    public UserController(UserManager<ApplicationUser>  userManager)
    {
      this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var users = await this.userManager.Users.ToListAsync();

      return View();

    }

    public async Task<IActionResult> Create()
    {
      var user = new ApplicationUser();
      user.UserName = "test";
      user.Email = "test@test.com";

     var result =  await this.userManager.CreateAsync(user, "Test12345");

      if(result.Succeeded)
      {
        // role ata
        await this.userManager.AddToRoleAsync(user, "Admin");
        // user claim tablosuna kayıt atıyor.
        await this.userManager.AddClaimAsync(user, new Claim("DateOfBirth", DateTime.Now.AddYears(-35).ToShortDateString()));
        await this.userManager.AddClaimAsync(user, new Claim("LinkedInAddress", "www.a.com"));
      }

      return View();
    }
  }
}
