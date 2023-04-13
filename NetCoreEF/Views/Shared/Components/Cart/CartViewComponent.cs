using Microsoft.AspNetCore.Mvc;
using NetCoreEF.Services;

namespace NetCoreEF.Views.Shared.Components.Cart
{
  public class CartViewComponent : ViewComponent
  {
    private readonly ICartService cartService;

    public CartViewComponent(ICartService cartService)
    {
      this.cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      return View(this.cartService.GetItems);
    }
  }
}
