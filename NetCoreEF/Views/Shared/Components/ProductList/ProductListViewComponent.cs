using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Data;

namespace NetCoreEF.Views.Shared.Components.ProductList
{
  public class ProductListViewComponent : ViewComponent
  {
    private readonly NorthwindContext db;

    public ProductListViewComponent(NorthwindContext db)
    {
      this.db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? searchText, int? categoryId)
    {

      //if(!string.IsNullOrEmpty(searchText))
      //{
      //  return View(await db.Products.Include(x=> x.Category).Where(x=> EF.Functions.Like(x.ProductName,searchText)).ToListAsync());
      //}
      //else
      //{
      //  return View(await db.Products.Include(x => x.Category).ToListAsync());
      //}

      //var query = db.Products.Include(x => x.Category).FirstOrDefaultAsync();

      var query = db.Products.Include(x => x.Category).AsQueryable();


      //var model = await query.ToListAsync();

      // dto çekmemiz lazım
      //var model = db.Products.Include(x => x.Category).ToList();

      if (!string.IsNullOrEmpty(searchText))
      {
        query = query.Where(x => x.ProductName.Trim().ToLower().Contains(searchText.Trim().ToLower()));

      }

      if(categoryId != null)
      {
        query = query.Where(x => x.CategoryId == categoryId);
      }

      //var model 

      return View(await query.ToListAsync());

    }
  }
}
