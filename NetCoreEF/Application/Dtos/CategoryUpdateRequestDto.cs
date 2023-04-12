using MediatR;

namespace NetCoreEF.Application.Dtos
{
  public class CategoryUpdateRequestDto:IRequest<CategoryUpdateResponseDto>
  {
    public string Key { get; set; }
    public int CategoryId { get; set; } // protectedKey

    public string CategoryName { get; set; }
    public string Description { get; set; }

  }
}
