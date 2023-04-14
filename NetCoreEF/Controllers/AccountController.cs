using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEF.Data.Identity;
using NetCoreEF.Models;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace NetCoreEF.Controllers
{
  public class AccountController : Controller
  {
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
      var model = new LoginDto
      {
        Email = "test@test.com",
        Password = "23232"
      };

      return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
      var user = await this.userManager.FindByEmailAsync(model.Email);
      var roles = await this.userManager.GetRolesAsync(user);
      var roleClaims = await this.roleManager.GetClaimsAsync(new ApplicationRole { Name = roles[0] });

      // custom login
      //var claims = new List<Claim>();
      //claims.Add(new Claim(ClaimTypes.Name, user.Email));
      //claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
      //claims.Add(new Claim(ClaimTypes.Role, string.Join(",", roles) ));
      //// custom login işlemi
      //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      //ClaimsPrincipal principal = new ClaimsPrincipal(identity);

      //var authProps = new AuthenticationProperties();
      //authProps.AllowRefresh = true;
      //authProps.ExpiresUtc = DateTimeOffset.Now.AddDays(1);
      //await HttpContext.SignInAsync(principal,authProps);

      // cookie kullanıcı değeri tanımlandık.
      await this.signInManager.SignInAsync(user, true);

      return Redirect("/");
    }

    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> LogOut()
    {
      // custom signOut
      // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      // cookie kullanıcı değeri tanımlandık.
      await this.signInManager.SignOutAsync();

      return Redirect("/");
    }

    public async Task<IActionResult> AccessDenied()
    {
      return View();
    }
  }
}
