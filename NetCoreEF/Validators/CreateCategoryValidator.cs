using FluentValidation;
using NetCoreEF.Models;

namespace NetCoreEF.Validators
{
  public class CreateCategoryValidator:AbstractValidator<CategoryCreateDto>
  {
    public CreateCategoryValidator()
    {
      RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Kategori adı boş geçilemez");
      RuleFor(x => x.Description).MaximumLength(200).WithMessage("Açıklama alanı en fazla 200 karakter olabilir");
      RuleFor(x => x.Name).Must(CategoryNameHasSpecialChars).WithMessage("Kategori ismi özel noktalama işareti içeremez");

    }

    private bool CategoryNameHasSpecialChars(string name)
    {
      foreach (var ch in name.ToCharArray())
      {
        if (char.IsPunctuation(ch))
          return false;
      }

      return true;
    }
  }
}
