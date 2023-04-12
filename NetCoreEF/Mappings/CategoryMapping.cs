using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using NetCoreEF.Data;
using NetCoreEF.Models;

namespace NetCoreEF.Mappings
{
  public class CategoryMapping:Profile
  {
   

    public CategoryMapping()
    {
      CreateMap<Category, CategoryDto>();
    }
  }
}
