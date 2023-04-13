using Microsoft.AspNetCore.Identity;

namespace NetCoreEF.Data.Identity
{
  public class ApplicationUser:IdentityUser
  {
    public string? WebSite { get; set; }

  }
}
