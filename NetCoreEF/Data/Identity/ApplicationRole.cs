using Microsoft.AspNetCore.Identity;

namespace NetCoreEF.Data.Identity
{
  public class ApplicationRole:IdentityRole
  {
    public string? Description { get; set; }

  }
}
