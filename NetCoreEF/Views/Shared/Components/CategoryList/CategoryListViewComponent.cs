using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Data;

namespace NetCoreEF.Views.Shared.Components.CategoryList
{
  public class CategoryListViewComponent : ViewComponent
  {
    private readonly NorthwindContext db;

    public CategoryListViewComponent(NorthwindContext db)
    {
      this.db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      var model = await db.Categories.Include(x=> x.Products).ToListAsync();

      return View(model);
    }
  }
}
