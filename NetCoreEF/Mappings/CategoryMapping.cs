using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using NetCoreEF.Application.Dtos;
using NetCoreEF.Data;
using NetCoreEF.Models;

namespace NetCoreEF.Mappings
{
  public class CategoryMapping:Profile
  {
   

    public CategoryMapping()
    {
      CreateMap<Category, CategoryDto>(); // arayüze dto çağırmam lazım bu sebeple entity to dto mapping
      //CreateMap<CategoryCreateDto, Category>(); // dto üzerinde validasyon kontrolü yaptıktan sonra to entity mapping

      CreateMap<Category, CategoryUpdateRequestDto>().ReverseMap();

      CreateMap<Category, CategoryCreateDto>()
        .ForMember(dest =>
            dest.Name,
            opt => opt.MapFrom(src => src.CategoryName))
        .ForMember(dest =>
            dest.Description,
            opt => opt.MapFrom(src => src.Description))
        .ReverseMap(); // yukarıdaki mapping işlemini reverMap ile terse çevirdik.
    }
  }
}
