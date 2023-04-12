namespace NetCoreEF.Application.Exceptions
{
  public class SameCategoryExistException:ApplicationException
  {
    public SameCategoryExistException():base("Aynı kategoriden mevcut! farklı bir isim veriniz")
    {

    }
  }
}
