using NetCoreEF.Data;
using NetCoreEF.Extensions;
using NetCoreEF.Models;

namespace NetCoreEF.Services
{
  public class CartService : ICartService
  {
    // httpContext class üzerinden erişim sağlayan interface
    private readonly IHttpContextAccessor httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
      this.httpContextAccessor = httpContextAccessor;
    }

   

    public List<CartDto> GetItems
    {
      

      get { 
      
        var cartItems = this.httpContextAccessor.HttpContext.Session.GetObject<List<CartDto>>("cart");

        if (cartItems == null)
          return new List<CartDto>();

        return cartItems;

      }
    }

    public void AddToCart(CartDto product)
    {
     
      var sessionCarItems = this.httpContextAccessor.HttpContext.Session.GetObject<List<CartDto>>("cart");

      if (sessionCarItems != null)
      {
        sessionCarItems.Add(product);

        httpContextAccessor.HttpContext.Session.SetObject<List<CartDto>>("cart",sessionCarItems);
      }
      else
      {
        sessionCarItems = new List<CartDto>();
        sessionCarItems.Add(product);
        httpContextAccessor.HttpContext.Session.SetObject<List<CartDto>>("cart", sessionCarItems);

      }
     
    }
  }
}
