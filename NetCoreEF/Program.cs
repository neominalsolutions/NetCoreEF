using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Application.Handlers;
using NetCoreEF.Attributes;
using NetCoreEF.Data;
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
