using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Application.Handlers;
using NetCoreEF.Attributes;
using NetCoreEF.Data;
using NetCoreEF.Data.Identity;
using NetCoreEF.Models;
using NetCoreEF.Services;
using NetCoreEF.Validators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// mvc modelstateler fluent validation üzerinden çalýþacaktýr.
builder.Services.AddControllersWithViews().AddFluentValidation(c=> c.RegisterValidatorsFromAssemblyContaining<CreateCategoryValidator>());

var assemby = Assembly.GetExecutingAssembly();
builder.Services.AddAutoMapper(assemby);

// reflection ile automapper eklemiþ olduk

// uygulama genelinde global olarak attibute üzerinden bir validayon kontrolü yapýcam
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ExceptionFilterAttribute>();

// mediatR uygulamada çalýþmasý için servisleri ekleri
// mediator kullanýlan 1 sýnýfý referans alarak ilgili katmaný reflection ile load edelim.
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<CategoryUpdateRequestHandler>());


// db baðlantýsý sadece buradan yönetiliyor.
// birden fazla db varsa istediðimiz kadar DbContext ekleyebiliriz
// scoped Service web request bazlý instance alýyor.// using blogu içeerisinde tanýmlar, disposeable olsun. her bir controlller request bazlý farklý instance çalýþýr.
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NortwindConn")));

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NortwindConn")));



#region CookieAuthentication

builder.Services.AddAuthentication(x =>
{
  // Cookies denlen bir þema ile 
  x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
{

  x.LoginPath = "/Account/Login";
  x.LogoutPath = "/Account/LogOut";
  x.AccessDeniedPath = "/Account/AccessDenied";
  x.SlidingExpiration = true; // her bir günde bir eðer cookie expire olmamoþ ise oturumu 1 gün daha yenile, oturumdan çýkýþ yap demeyene kadar cookie yenilenecek.
  x.ExpireTimeSpan = TimeSpan.FromDays(1); // 1 gün


});

//builder.Services.AddAuthentication().AddCookie(x => {

//  x.LoginPath = "/Account/Login";
//  x.LogoutPath = "/Account/LogOut";
//  x.AccessDeniedPath = "/Account/AccessDenied";
//  x.SlidingExpiration = true; // her bir günde bir eðer cookie expire olmamoþ ise oturumu 1 gün daha yenile, oturumdan çýkýþ yap demeyene kadar cookie yenilenecek.
//  x.ExpireTimeSpan = TimeSpan.FromDays(1); // 1 gün


//});

//builder.Services.AddScoped<UserManager<ApplicationUser>>();
//builder.Services.AddScoped<SignInManager<ApplicationUser>>();
//builder.Services.AddScoped<RoleManager<ApplicationRole>>();
#endregion

builder.Services.AddAuthorization(opt =>
{
  // hem Admin, Hem ProductReadOnly (RoleClaim veya UseClaim)
  opt.AddPolicy("ProductReadOnlyPolicy", policy =>
  {
    policy.RequireAuthenticatedUser();
    policy.RequireRole("Admin");
    policy.RequireClaim("ProductReadOnly", "ReadOnly");
  });

  opt.AddPolicy("ProductWritePolicy", policy =>
  {
    policy.RequireAuthenticatedUser();
    policy.RequireRole("Admin");
    policy.RequireClaim("ProductWrite", "Delete", "Update", "Create"); // sistem login olurken sadece user info user claim ve role bilgilerini cookie bastý

  });

  opt.AddPolicy("ProductUpdateOnlyPolicy", policy =>
  {
    policy.RequireClaim("ProductUpdateOnly", "Update");
  });
});


// sistemdeki kullanýcý rol ve oturum ile ilgili ayarlarý yaptýðýmý kýsým
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
  opt.User.RequireUniqueEmail = true;
  opt.Password.RequireDigit = true;
  opt.Password.RequiredLength = 8;
  opt.Password.RequireUppercase = true;
  opt.Password.RequireLowercase = true;
  opt.Password.RequireNonAlphanumeric = false;
  opt.SignIn.RequireConfirmedEmail = false;
  opt.SignIn.RequireConfirmedPhoneNumber = false;

}).AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddTransient<ICartService, CartService>();

#region SessionInMemory

//builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
// class üzerinden HttpContext baðlanýyoruz.
builder.Services.AddHttpContextAccessor();

#endregion



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

// sesion middleware sürece ekledik.
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // kimlik doðrulamasý middleware çalýþtýr.
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
  endpoints.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
  );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
