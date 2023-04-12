using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Attributes;
using NetCoreEF.Data;
using NetCoreEF.Models;
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

// db baðlantýsý sadece buradan yönetiliyor.
// birden fazla db varsa istediðimiz kadar DbContext ekleyebiliriz
// scoped Service web request bazlý instance alýyor.// using blogu içeerisinde tanýmlar, disposeable olsun. her bir controlller request bazlý farklý instance çalýþýr.
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NortwindConn")));


var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
