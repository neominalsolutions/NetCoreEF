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
// mvc modelstateler fluent validation �zerinden �al��acakt�r.
builder.Services.AddControllersWithViews().AddFluentValidation(c=> c.RegisterValidatorsFromAssemblyContaining<CreateCategoryValidator>());

var assemby = Assembly.GetExecutingAssembly();
builder.Services.AddAutoMapper(assemby);

// reflection ile automapper eklemi� olduk

// uygulama genelinde global olarak attibute �zerinden bir validayon kontrol� yap�cam
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ExceptionFilterAttribute>();

// mediatR uygulamada �al��mas� i�in servisleri ekleri
// mediator kullan�lan 1 s�n�f� referans alarak ilgili katman� reflection ile load edelim.
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<CategoryUpdateRequestHandler>());


// db ba�lant�s� sadece buradan y�netiliyor.
// birden fazla db varsa istedi�imiz kadar DbContext ekleyebiliriz
// scoped Service web request bazl� instance al�yor.// using blogu i�eerisinde tan�mlar, disposeable olsun. her bir controlller request bazl� farkl� instance �al���r.
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NortwindConn")));

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NortwindConn")));



#region CookieAuthentication

builder.Services.AddAuthentication(x =>
{
  // Cookies denlen bir �ema ile 
  x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
{

  x.LoginPath = "/Account/Login";
  x.LogoutPath = "/Account/LogOut";
  x.AccessDeniedPath = "/Account/AccessDenied";
  x.SlidingExpiration = true; // her bir g�nde bir e�er cookie expire olmamo� ise oturumu 1 g�n daha yenile, oturumdan ��k�� yap demeyene kadar cookie yenilenecek.
  x.ExpireTimeSpan = TimeSpan.FromDays(1); // 1 g�n


});

//builder.Services.AddAuthentication().AddCookie(x => {

//  x.LoginPath = "/Account/Login";
//  x.LogoutPath = "/Account/LogOut";
//  x.AccessDeniedPath = "/Account/AccessDenied";
//  x.SlidingExpiration = true; // her bir g�nde bir e�er cookie expire olmamo� ise oturumu 1 g�n daha yenile, oturumdan ��k�� yap demeyene kadar cookie yenilenecek.
//  x.ExpireTimeSpan = TimeSpan.FromDays(1); // 1 g�n


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
    policy.RequireClaim("ProductWrite", "Delete", "Update", "Create"); // sistem login olurken sadece user info user claim ve role bilgilerini cookie bast�

  });

  opt.AddPolicy("ProductUpdateOnlyPolicy", policy =>
  {
    policy.RequireClaim("ProductUpdateOnly", "Update");
  });
});


// sistemdeki kullan�c� rol ve oturum ile ilgili ayarlar� yapt���m� k�s�m
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
// class �zerinden HttpContext ba�lan�yoruz.
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

// sesion middleware s�rece ekledik.
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // kimlik do�rulamas� middleware �al��t�r.
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
