using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetCoreEF.Data.Identity
{
    public class AppIdentityDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> dbContextOptions):base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);


      builder.Entity<ApplicationUser>().ToTable("Users");
      builder.Entity<ApplicationRole>().ToTable("Roles");
      builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
      builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
      builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
      builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
      builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

    }


  }
}
