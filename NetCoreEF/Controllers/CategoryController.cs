using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Data;
using NetCoreEF.Models;

namespace NetCoreEF.Controllers
{
  public class CategoryController : Controller
  {
    private readonly NorthwindContext db;
    public IDataProtector _protector;
    private IMapper mapper;

    public CategoryController(NorthwindContext db, IDataProtectionProvider provider, IMapper mapper)
    {
      this.db = db;
      this._protector = provider.CreateProtector("kategoriId");
      this.mapper = mapper;
    }



    [HttpGet("kategoriler")]
    public async Task<IActionResult> List()
    {
      //var model = await db.Categories.Select(a => new CategoryDto
      //{
      //  CategoryName = a.CategoryName,
      //  ProtectorId = this._protector.Protect(a.CategoryId.ToString()),
      //  Description = a.Description

      //}).ToListAsync();
      var entities = await db.Categories.ToListAsync();
      var model = mapper.Map<List<CategoryDto>>(entities);
      model.ForEach((item) =>
      {
        item.ProtectorId = this._protector.Protect(item.CategoryId.ToString());
      });


      return View(model);
    }

    //[HttpGet]
    //public IActionResult List()
    //{
    //  // async olmayan bir action method içerisinde async bir methodu çağırma şekli
    //  var model =  db.Categories.ToListAsync().GetAwaiter().GetResult();

    //  return View(model);
    //}

    [HttpGet]
    [Route("kategori/{cId}", Name = "kategoriDetay")]
    public async Task<IActionResult> Detail(string cId)
    {
      string entityId = _protector.Unprotect(cId);
      int id = Convert.ToInt32(entityId);

      if (cId == null)
        return BadRequest();  // 400 doğru bir parametre değer gelmedi

      var entity = await db.Categories.FirstOrDefaultAsync(x=> x.CategoryId == id);

      var model = mapper.Map<CategoryDto>(entity);

      if (model == null)
        return NotFound(); // 404Page döner



      return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
      return View();
    }

    [HttpGet]
    public IActionResult Update(int? id)
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Category category)
    {
      return View();
    }

    public IActionResult Delete(int? id)
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Category category)
    {
      return View();
    }

  



  }
}
