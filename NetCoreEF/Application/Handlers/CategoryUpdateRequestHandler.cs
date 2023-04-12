using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NetCoreEF.Application.Dtos;
using NetCoreEF.Application.Exceptions;
using NetCoreEF.Data;
using System.Security.Cryptography;

namespace NetCoreEF.Application.Handlers
{
  public class CategoryUpdateRequestHandler : IRequestHandler<CategoryUpdateRequestDto, CategoryUpdateResponseDto>
  {
    private readonly IMapper mapper;
    private readonly NorthwindContext db;
    public IDataProtector protector;

    public CategoryUpdateRequestHandler(IMapper mapper, NorthwindContext db, IDataProtectionProvider provider)
    {

      this.mapper = mapper;
      this.db = db;
      protector = provider.CreateProtector("kategoriId");
    }


    public async Task<CategoryUpdateResponseDto> Handle(CategoryUpdateRequestDto request, CancellationToken cancellationToken)
    {
      

      string entityId = protector.Unprotect(request.Key.ToString());
      int id = Convert.ToInt32(entityId);
      request.CategoryId = id;

      //var entity = db.Categories.Find(id);
      //entity.CategoryName = request.CategoryName;

     

      var entity = mapper.Map<Category>(request);
      var response = new CategoryUpdateResponseDto();

      // kategori name unique name yapıyoruz. yanlışlıkla var olan bir kategori ismi ile güncellememek için yaptık
     var sameNameEntity = await db.Categories.FirstOrDefaultAsync(x => x.CategoryName == request.CategoryName && x.CategoryId != request.CategoryId);

      if(sameNameEntity != null)
      {
        throw new SameCategoryExistException();
      }

      db.Categories.Update(entity);

      int result = await db.SaveChangesAsync();

      if(result > 0)
      {
        response.IsSucceded = true;
        response.ErrorMessage = string.Empty;
        response.SuccessMessage = $"{request.CategoryName} kaydı başarılı bir şekilde güncellendi";
      }
      else
      {
        response.ErrorMessage = "Kayıt güncellenmeni";
      }

      return response;
    }
  }
}
