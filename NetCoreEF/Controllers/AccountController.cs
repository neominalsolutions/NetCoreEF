using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEF.Data.Identity;
using System.Security.Claims;

namespace NetCoreEF.Controllers
{
  public class AccountController : Controller
  {
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
    }

    public async Task<IActionResult> Login()
    {
      var user = await this.userManager.FindByEmailAsync("test@test.com");

      // custom login
      //var claims = new List<Claim>();
      //claims.Add(new Claim("UserName", "ali"));
      //claims.Add(new Claim("UserId", "32432432"));

      // custom login işlemi
      //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      //ClaimsPrincipal principal = new ClaimsPrincipal(identity);

      //var authProps = new AuthenticationProperties();
      //authProps.AllowRefresh = true;
      //authProps.ExpiresUtc = DateTimeOffset.Now.AddDays(1);
      //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

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
  }
}
