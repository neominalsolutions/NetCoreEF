using Microsoft.AspNetCore.Mvc;
using NetCoreEF.Models;
using NetCoreEF.Services;

namespace NetCoreEF.Controllers
{
  public class ProductController : Controller
  {
    private readonly ICartService cartService;

    public ProductController(ICartService cartService)
    {
      this.cartService = cartService;
    }


    [HttpGet]
    public async Task<IActionResult> ProductListViewComponent(string? searchText)
    {
      return ViewComponent("ProductList", new { searchText });
      // html result döndürecek
    }

    public IActionResult Index(int? cId)
    {
      ViewBag.cId = cId;
      // var model = db.categories.tolist();

      return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] CartDto model)
    {
      this.cartService.AddToCart(model);

      return ViewComponent("Cart");

    }
  }
}
