using NetCoreEF.Data;
using NetCoreEF.Models;

namespace NetCoreEF.Services
{
  public interface ICartService
  {
    void AddToCart(CartDto product);
    List<CartDto> GetItems { get; }
  }
}
